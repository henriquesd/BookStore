# BookStore
Web API Project using .NET Core 3.1 and SPA project using Angular 9

## Technologies
- .NET Core 3.1
- Entity Framework 3.1
- Fluent API
- AutoMapper
- Swagger
- SQL Server

### Unit tests
- xUnit
- Moq
- SQLite In-Memory database

### SPA
- Angular 9
- Ng-Bootstrap
- Ngx-Toastr

## Architecture
- 3 Layers:
  - Application layer (API)
    - Controllers
    - Dtos
  - Domain layer
    - Models
    - Interfaces
    - Services
  - Infrastructure layer
    - Repository Pattern
- SPA

## Articles
On the series of articles, "Creating an Application from Scratch using .NET Core and Angular", there is the explanation step by step about how this code was implemented:
- [Part 1: Creating the initial structure](https://henriquesd.medium.com/creating-an-application-from-scratch-using-net-core-and-angular-part-1-d1c66733c57d)
- [Part 2: Implementing the Models and creating the Database with EF Core](https://henriquesd.medium.com/creating-an-application-from-scratch-using-net-core-and-angular-part-2-95e67eebadde)
- [Part 3: Implementing the Service classes and the Repository Pattern](https://henriquesd.medium.com/creating-an-application-from-scratch-using-net-core-and-angular-part-3-e3c42cd9cc01)
- [Part 4: Implementing the API layer](https://henriquesd.medium.com/creating-an-application-from-scratch-using-net-core-and-angular-part-4-8718e3f529aa)
- [Part 5: Implementing the SPA](https://henriquesd.medium.com/creating-an-application-from-scratch-using-net-core-and-angular-part-5-ab1ac4cd5609)
- [Part 6: Implementing Unit Tests for the Domain Layer](https://henriquesd.medium.com/creating-an-application-from-scratch-using-net-core-and-angular-part-6-76daa358db41)
- [Part 7: Implementing Unit Tests for the API layer](https://henriquesd.medium.com/creating-an-application-from-scratch-using-net-core-and-angular-part-7-8b7f77772b36)
- [Part 8: Implementing Unit Tests for the Infrastructure Layer](https://henriquesd.medium.com/creating-an-application-from-scratch-using-net-core-and-angular-part-8-85018dc84429)

## Docker

### Portainer
To make the management of the containers, Portainer (https://www.portainer.io/) can be used. Portainer is also a docker image.
To Install using these two commands:

`docker volume create portainer_data`

`docker run -d -p 8000:8000 -p 9000:9000 --name=portainer --restart=always -v /var/run/docker.sock:/var/run/docker.sock -v portainer_data:/data portainer/portainer-ce`

#### Accessing Portainer
Portainer can be access on: http://localhost:9000/

The username is admin and the password is admin.

On Portainer, select 'local', and go to 'Containers' menu to see the containers.

### Running the application with Docker
To run the containers, access the docker folder (BookStore\docker) in cmd and execute the command:

`docker-compose -f bookstore_production.yml up --build`


To access the API, go to: http://localhost:5001/swagger/index.html

To connect into the database, open SQL Server Management Studio and in the Server name use 'localhost' and use the same username and password that is in the 'bookstore_production.yml' file (it will take some seconds until the database be created).
