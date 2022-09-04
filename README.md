1- Realizar un git clone https://github.com/RomVeron/ApiTest.git Del repositorio ApiTest
2- Abrir el proyecto con un IDE Visual Studio 2022 o JetBrains Rider 2022
3- Ejecutar el proyecto e abrir el navegador https://localhost:5001/swagger/index.html
4- Utilizar el metodo POST https://localhost:5001/api/token para obtener el JWT con el siguiente USUARIO de prueba
{
  "userName": "admin@test.com",
  "password": "P@ssword"
}

5- Ejecutar los siguientes metodos para consulta e insersion