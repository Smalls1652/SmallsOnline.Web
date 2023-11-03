param location string = resourceGroup().location
param randomHash string = newGuid()

param baseResourceName string

param vnetAddressSpace string = '10.0.0.0/16'
param vnetSubnetAddressSpace string = '10.0.0.0/23'

param registryResourceGroupName string
param registryName string
param imageTag string

param cosmosDbResourceGroupName string
param cosmosDbName string
param cosmosDbContainerName string

var randomString = take(uniqueString(subscription().id, resourceGroup().id, randomHash), 4)
var resourceName = '${baseResourceName}-${randomString}'

resource containerRegistry 'Microsoft.ContainerRegistry/registries@2023-07-01' existing = {
  name: registryName
  scope: resourceGroup(registryResourceGroupName)
}

resource cosmosDbResource 'Microsoft.DocumentDB/databaseAccounts@2023-09-15' existing = {
  name: cosmosDbName
  scope: resourceGroup(cosmosDbResourceGroupName)
}

resource virtualNetwork 'Microsoft.Network/virtualNetworks@2023-05-01' = {
  #disable-next-line use-stable-resource-identifiers
  name: '${resourceName}-vnet'
  location: location

  properties: {
    addressSpace: {
      addressPrefixes: [
        vnetAddressSpace
      ]
    }

    subnets: []
  }
}

module virtualNetworkSubnetDeployment 'public-site-vnet.bicep' = {
  name: 'create_${baseResourceName}-vnet-subnet'
  scope: resourceGroup()
  dependsOn: [
    virtualNetwork
  ]

  params: {
    baseResourceName: resourceName
    vnetSubnetAddressSpace: vnetSubnetAddressSpace
  }
}

resource containerAppEnvironment 'Microsoft.App/managedEnvironments@2023-05-01' = {
  #disable-next-line use-stable-resource-identifiers
  name: '${resourceName}-env'
  location: location

  dependsOn: [
    virtualNetworkSubnetDeployment
  ]

  properties: {
    workloadProfiles: [
      {
        name: 'Consumption'
        workloadProfileType: 'Consumption'
      }
    ]

    vnetConfiguration: {
      infrastructureSubnetId: virtualNetworkSubnetDeployment.outputs.subnetResourceId
      internal: false
    }

    zoneRedundant: false
  }
}

resource containerApp 'Microsoft.App/containerApps@2023-05-01' = {
  #disable-next-line use-stable-resource-identifiers
  name: '${resourceName}-app'
  location: location

  properties: {
    environmentId: containerAppEnvironment.id
    configuration: {
      ingress: {
        targetPort: 8080
        transport: 'auto'
        external: false
      }

      activeRevisionsMode: 'Single'

      registries: [
        {
          server: containerRegistry.properties.loginServer
          username: containerRegistry.listCredentials().username
          passwordSecretRef: 'registry-pwd'
        }
      ]

      secrets: [
        {
          name: 'registry-pwd'
          value: containerRegistry.listCredentials().passwords[0].value
        }
        {
          name: 'cosmosdb-connectionstring'
          value: cosmosDbResource.listConnectionStrings().connectionStrings[2].connectionString
        }
      ]
    }

    template: {
      containers: [
        {
          name: 'publicsite-app'
          image: '${containerRegistry.properties.loginServer}/${imageTag}'

          resources: {
            #disable-next-line BCP036
            cpu: '0.5'
            memory: '1Gi'
          }

          env: [
            {
              name: 'CosmosDbContainerName'
              value: cosmosDbContainerName
            }

            {
              name: 'CosmosDbConnectionString'
              secretRef: 'cosmosdb-connectionstring'
            }
          ]

          probes: [
            {
              type: 'Liveness'
              periodSeconds: 10

              httpGet: {
                port: 8080
                path: '/healthz'
              }
            }

            {
              type: 'Readiness'
              periodSeconds: 10

              httpGet: {
                port: 8080
                path: '/healthz'
              }
            }

            {
              type: 'Startup'
              periodSeconds: 10

              httpGet: {
                port: 8080
                path: '/healthz'
              }
            }
          ]
        }
      ]
    }

    workloadProfileName: 'Consumption'
  }
}
