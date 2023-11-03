param baseResourceName string

param vnetSubnetAddressSpace string = '10.0.0.0/23'

resource virtualNetwork 'Microsoft.Network/virtualNetworks@2023-05-01' existing = {
  name: '${baseResourceName}-vnet'
  scope: resourceGroup()
}

resource virtualNetworkSubnet 'Microsoft.Network/virtualNetworks/subnets@2023-05-01' = {
  name: 'containerapps-infra'
  parent: virtualNetwork

  properties: {
    addressPrefix: vnetSubnetAddressSpace

    delegations: [
      {
        name: 'Microsoft.App.environments'
        type: 'Microsoft.Network/availableDelegations'
        properties: {
          serviceName: 'Microsoft.App/environments'
        }
      }
    ]
  }
}

output subnetResourceId string = virtualNetworkSubnet.id
