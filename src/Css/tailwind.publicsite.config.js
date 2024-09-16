/** @type {import('tailwindcss').Config} */
module.exports = {
  "content": [
    "../Lib.Components/**/*.{razor,cshtml,html}",
    "../PublicSite/Server/**/*.{razor,cshtml,html}"
  ],
  "theme": {
    "container": {
      "padding": {
        "DEFAULT": "1rem",
        "sm": "1rem",
        "lg": "3rem",
        "xl": "7rem",
        "2xl": "8rem"
      }
    },
    "extend": {},
  },
  "plugins": [],
}

