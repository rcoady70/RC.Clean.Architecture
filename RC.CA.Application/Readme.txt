This project contains our 

- Application logic.
- Some contracts implemented in core but mist will be implemented in the infrastructure
- Automapper is used to map between entities
    -AutoMapper
    -AutoMapper.Extensions.Microsoft.DependencyInjection
- Mediatr used to handle events for example getalluserbylastlogin 
    -MediatR.Extensions.Microsoft.DependencyInjection

- Validation flow
   
  Command ----->  Request Handler   ------> Response
                  Validation Here
