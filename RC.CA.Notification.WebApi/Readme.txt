EMail notifdication service
-----------------------------
- API call drops request onto the azure bus 
- Notifcations are processed from azure bus
- Generic process processes the messages and sends email through sendgrids email api
- Uses auto-mapper to map data between request and event bus message 
- Api key stored in azure key-vault
- Healthcheck monitoring enabled
- 
