------------------------++++++++----------------------------------------

Se  carga el archivo evidencias en el cual se observan las  imagenes correspondientes para cada cada punto de la prueba y tambien indica como se puede usar los endpoint.
se crea la carpeta Querys_JSON en la cual podras encontar los JSON correspondientes con los datos de cada api estan nombrados segun cada  punto de la prueba
y encontraras un archivo SQL el cual re permitira crear la base de datos con la data respectiva.

IMPORTANTE----: 

Baase de datos : SQLServer.
se debe cambiar la conexion en el proyecto "EnergyDataUploader" para poder que se conecte la api con la base de datos, 
archivo appsetings.json : 

{
    "ConnectionStrings": {
        "MiConexion": "Server=FERCHO\\SQLEXPRESS;Database=DistribucionEnergia;Trusted_Connection=True;TrustServerCertificate=True;"
    },
    "Logging": {
        "LogLevel": {
            "Default": "Information",
            "Microsoft.AspNetCore": "Warning"
        }
    },
    "AllowedHosts": "*"
}

------- se debe cambiar el server por el correspòndiente en mi caso es Server=FERCHO\\SQLEXPRESS lo reeemplazas por el correspondiente a tu equipo.



--------------------------------------+++++++++++++---------------------------------------------------------