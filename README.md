# Sistema de Gestión de Tareas (PC03)

Este proyecto consiste en una solución API RESTful construida en ASP.NET Core para gestionar tareas internas y enriquecerlas con información externa, además de clasificar comentarios usando Machine Learning con ML.NET.

## Rama Actual
* **feature/api-tareas** (Pregunta 1: Creación de API RESTful y modelo de datos)

---

## 🚀 Pasos para Ejecutar Localmente

### Prerrequisitos
* [.NET 10 SDK](https://dotnet.microsoft.com/download) o superior.
* Herramienta `dotnet-ef` para la gestión de base de datos (configurada localmente).

### Ejecución de la API
1. Abre tu terminal en la raíz del proyecto.
2. Navega al directorio de la API y ejecuta la aplicación:
   ```bash
   cd GestionTareasApi
   dotnet run
   ```
3. La base de datos SQLite `tareas.db` se creará y migrará automáticamente en el primer arranque.
4. El servidor estará disponible en la dirección local indicada por la consola (ej. `http://localhost:5000` o `https://localhost:5001`).

---

## 💾 Comandos de Migración (Entity Framework Core)

Para gestionar la base de datos se utiliza la herramienta local `dotnet-ef`. Estos son los comandos clave ejecutados desde la raíz del espacio de trabajo:

* **Crear una nueva migración:**
  ```bash
  dotnet ef migrations add <NombreMigracion> --project GestionTareasApi
  ```
* **Aplicar migraciones a la base de datos manualmente:**
  ```bash
  dotnet ef database update --project GestionTareasApi
  ```
> *Nota: La aplicación está configurada para aplicar las migraciones pendientes de forma automática en el arranque mediante `Program.cs`.*

---

## 🛠️ Endpoints Implementados (API Tareas)

Todos los endpoints tienen el prefijo `/api/tareas`. Los Enums de **Estado** (`Pendiente`, `EnProceso`, `Completada`) y **Prioridad** (`Baja`, `Media`, `Alta`) se envían y reciben como cadenas de texto en formato JSON.

| Método | Endpoint | Descripción | Estado de Retorno |
| :--- | :--- | :--- | :--- |
| **GET** | `/api/tareas` | Obtiene la lista completa de tareas. | `200 OK` |
| **GET** | `/api/tareas/{id}` | Obtiene el detalle de una tarea por su ID. | `200 OK` o `404 Not Found` |
| **POST** | `/api/tareas` | Crea una nueva tarea en el sistema. | `201 Created` o `400 Bad Request` |
| **PUT** | `/api/tareas/{id}` | Actualiza completamente una tarea existente. | `204 No Content`, `400 Bad Request` o `404 Not Found` |
| **DELETE** | `/api/tareas/{id}` | Elimina una tarea del sistema por su ID. | `204 No Content` o `404 Not Found` |

### Validaciones de Entrada
* **Título**: Obligatorio.
* **Prioridad**: Obligatoria (Baja, Media, Alta).
* **Estado**: Obligatorio (Pendiente, EnProceso, Completada).
* **Fecha de Vencimiento**: Obligatoria, no puede ser menor a la fecha actual del sistema.

### Ejemplo de JSON para POST/PUT
```json
{
  "titulo": "Implementar endpoints RESTful",
  "descripcion": "Crear y testear los controladores para la gestión de tareas internas",
  "estado": "Pendiente",
  "prioridad": "Alta",
  "fechaVencimiento": "2026-06-15T00:00:00"
}
```

---

## 🌐 Consumo de API Externa (Planificado para siguientes fases)
*La integración con la API externa para enriquecer tareas se implementará en la rama correspondiente (`feature/consumo-api`).*

---

## 🧠 Modelo ML.NET (Planificado para siguientes fases)
*La clasificación de comentarios y recomendación inteligente utilizando ML.NET se implementará en la rama correspondiente (`feature/ml-net`).*
