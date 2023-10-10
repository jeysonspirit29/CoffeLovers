# CoffeLovers - Desafío técnico


## Contenido

- **Proyecto Backend**: 
	- Se creó una solución e implementó una solución con arquitectura limpia y buenas prácticas de desarrollo con .NET 7.
	- Se creó un proyecto para las pruebas unitarias con XUnit.
	> Frameworks y librerías usadas: EF Core, MediatR, Migrations, FluentValidations, Identity, ErrorOr, Automapper, Swagger, Moq y FluentAssertions.

- **Proyecto Frontend**
	- Se creó una aplicación web con Angular
	> Librerias usadas: Solo se utilizó angular puro para el proyecto, no se importaron paquetes de terceros.

La base de datos usada es InMemory Localhost.
## Features
|        #        |Caso de Uso|Roles                         |
|----------------|-------------------------------|-----------------------------|
| 1|Crear orden con maetro detalle.          |Usuario            |
| 2|Ver ordenes con maestro detalle     |Todos           |
|3|Enviar orden a cocina.     |Empleado           |
|4|Entregar orden a usuario.|Empleado|
|5|Cobrar orden|Supervisor, Administrador|
 **Importante**: Sobre el caso de uso 2:  
 
 - Los usuarios solo pueden las ordenes creadas por ellos mismos.
 - Los empleados pueden ver las ordenes pendientes de atender y las ordenes ya atendidas por ellos mismos.
 - Los supervisores solo pueden ver las ordenes de los empleados que tienen a cargo.
 - El administrador puede ver todas las ordenes.

 **Puede utilizar los siguiente usuarios pre-cargados**:
 - **Rol usuario**: user1, user2, user3, user4 y user5.
 - **Rol empleado**: employee1, employee2 y employee3.
 - **Rol supervisor**: supervisor1 (jefe de employee1 y employee2) y supervisor2 (jefe de employee3) .
 - **Rol administrador**: admin1.

La contraseña para todos es: abc123.
 	
## Diagrama Entidad Relación

![alt text](https://i.ibb.co/TgWyvNh/imagen-2023-10-10-163346285.png)

## Levantar el proyecto

- **Levantar Backend**: 
	- Abrir la solución backend con visual studio y presionar el botón RUN. En caso de usar comandos, ejecutar: **dotnet run**.
	- Para ejecutar las pruebas ir al proyecto "Application.UnitTests", clic derecho y seleccionar la opción "Run Tests". En caso de usar comandos , ejecutar: **dotnet test**.

- **Levantar Frontend**
	- Abrir solución frontend y ejecutar dos comandos:
	1. npm install
	2. npm start


