# 🏥 Sistema de Gestión de Citas Médicas - Backend

API RESTful desarrollada para la gestión integral de una clínica, permitiendo la administración de doctores, pacientes y reservas de citas con validaciones temporales complejas.

Este proyecto implementa **Arquitectura Hexagonal** para asegurar la escalabilidad y el desacoplamiento del código.

## 🚀 Tecnologías Principales
* **Lenguaje:** C# (.NET 8)
* **IDE:** Visual Studio 2022
* **Arquitectura:** Hexagonal (Dominio, Aplicación, Infraestructura)
* **Documentación:** Swagger (OpenAPI)
* **Base de Datos:** MySQL

## ✨ Funcionalidades Clave

### 1. 🕒 Validación de Horarios Complejos (Turnos Nocturnos)
El sistema cuenta con un motor de validación lógica capaz de gestionar turnos que cruzan la medianoche (ej. "Guardia Nocturna" de 22:00 PM a 06:00 AM).
* Algoritmo inteligente para detectar disponibilidad en cruces de día.
* Prevención de solapamiento de citas (Doble reserva).

### 2. 🌍 Manejo de Zonas Horarias (Global Timezone)
Arquitectura preparada para estándares internacionales:
* Almacenamiento de fechas en formato **UTC (Z)** en base de datos.
* El backend procesa y sirve los datos normalizados para que cualquier cliente (Frontend Web/Móvil) pueda convertirlos a su hora local sin errores de cálculo.

### 3. 🛡️ Arquitectura Hexagonal
El código está estructurado en capas para separar la lógica de negocio de los detalles técnicos:
* **Domain:** Entidades y reglas de negocio puras.
* **Application:** Casos de uso e interfaces.
* **Infrastructure:** Implementación de bases de datos y controladores API.

## ⚙️ Configuración de Base de Datos (MySQL)

El repositorio incluye el script de inicialización automática.

1.  Ubicar el archivo **`SistemaDeCitasMedicas.sql`** en la raíz de este proyecto.
2.  Abrir **MySQL Workbench** (o su gestor de preferencia).
3.  Ejecutar el script completo.
    * *Nota:* El script incluye la creación de la base de datos `medical_db` y la inserción de datos semilla (Doctores de prueba para turnos mañana, tarde y madrugada).
4.  Verificar la cadena de conexión en el archivo `appsettings.json` para que coincida con sus credenciales locales.

## 📚 Documentación de API (Swagger)

El proyecto incluye Swagger UI para pruebas interactivas. Endpoints disponibles:

### 👨‍⚕️ Doctores (Doctors)
* `GET /api/Doctors`: Listar staff médico y sus estados.
* `POST /api/Doctors`: Registrar nuevo especialista con turno definido.
* `PUT /api/Doctors/{id}`: Modificar datos o disponibilidad.
* `DELETE /api/Doctors/{id}`: Dar de baja.

### 📅 Citas (Appointments)
* `POST /api/Appointments`: Reservar cita (Incluye validaciones de negocio).
* `GET /api/Appointments`: Historial de reservas.
* `PUT /api/Appointments/{id}`: Reprogramación de fecha/hora.
* `PATCH /api/Appointments/{id}/status`: Cambiar estado (Programada -> Atendida/Cancelada).

## 🛠️ Instalación y Ejecución

1.  Clonar el repositorio:
    ```bash
    git clone [https://github.com/LucasAED/DSW1_PROYECTO_FINAL_SISTEMAS_DE_CITAS_MEDICAS_BACKEND.git](https://github.com/LucasAED/DSW1_PROYECTO_FINAL_SISTEMAS_DE_CITAS_MEDICAS_BACKEND.git)
    ```
2.  Abrir el archivo `.sln` (Solución) con **Visual Studio 2022**.
3.  **Restauración de Paquetes:**
    * Visual Studio 2022 debería detectar y descargar las dependencias automáticamente.
    * *Si esto no ocurre:* Ir al menú **Herramientas** > **Administrador de paquetes NuGet** > **Consola del administrador de paquetes** y ejecutar:
        ```bash
        dotnet restore
        ```
    * *Opción visual:* Dar clic derecho sobre la "Solución" en el explorador de archivos y seleccionar **"Restaurar paquetes NuGet"**.
4.  Configurar la cadena de conexión en `appsettings.json` (verificar user/password de su MySQL local).
5.  Presionar **F5** o el botón "Iniciar" para compilar y ejecutar.

---
**Autor:** Lucas Alonso Escalante Delgado
**Curso:** Desarrollo de Servicios Web I (DSW1)
