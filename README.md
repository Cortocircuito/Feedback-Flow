# Feedback Flow

**Feedback Flow** es una aplicación de escritorio desarrollada en C# WinForms (.NET 10) diseñada para automatizar la distribución de feedback personalizado a estudiantes.

## 🚀 Características

- **Gestión Automatizada**: Organiza el feedback en carpetas diarias (`YYYYMMDD`) dentro de `Mis Documentos/Feedback-Flow`.
- **Carga de Datos**: Importa la lista de alumnos desde un archivo `alumnos.csv`.
- **Generación de PDF**: Convierte notas de texto individuales (`.txt`) en documentos PDF profesionales utilizando iText 9.
- **Integración con Email**: Genera borradores de correo electrónico (`.eml`) con el contenido de la clase y el feedback personalizado adjuntos, listos para ser revisados y enviados desde Outlook u otro cliente de correo.
- **Arquitectura Moderna**: Utiliza Inyección de Dependencias para un código limpio y mantenible.

## 📋 Requisitos

- **Entorno**: Windows OS.
- **Runtime**: .NET 10.0 SDK/Runtime.
- **Software**: Cliente de correo (Outlook recomendado) para abrir los archivos `.eml`.
- **Librerías principales**:
  - `iText 9`: Para la manipulación y generación de PDFs.
  - `MimeKit`: Para la creación de mensajes de correo electrónico.
  - `Microsoft.Extensions.DependencyInjection`: Para la gestión de servicios.

## 🛠️ Estructura del Proyecto

```text
Feedback Flow/
├── Models/             # Modelos de datos (Student)
├── Services/           # Lógica de negocio
│   ├── Interfaces/     # Contratos de servicios
│   ├── CsvDataService.cs
│   ├── FileSystemService.cs
│   ├── OutlookEmailService.cs
│   └── PdfGenerationService.cs
├── alumnos.csv         # Archivo de datos de alumnos (ejemplo)
└── Program.cs          # Punto de entrada y configuración de DI
```

## 📖 Cómo usar

1. **Configuración Inicial**:
   - Asegúrate de tener un archivo `alumnos.csv` en la raíz del proyecto o en la carpeta de la aplicación con el formato: `Nombre,Apellido,Email`.
2. **Ejecución**:
   - Inicia la aplicación. Se creará automáticamente la carpeta del día en sus documentos.
3. **Procesamiento**:
   - Selecciona el PDF con el contenido general de la clase.
   - Coloca los archivos `.txt` con las notas de cada alumno en sus carpetas correspondientes (creadas automáticamente).
   - Haz clic en **Generate** para crear los documentos y los borradores de correo.
4. **Envío**:
   - Revisa los borradores abiertos en tu cliente de correo y envíalos.

## 🛠️ Desarrollo

Para compilar el proyecto localmente:

```powershell
dotnet restore
dotnet build
dotnet run
```

---
Desarrollado para simplificar el flujo de trabajo docente.
