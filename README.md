# 🔎 ElasticPDF

> **Busca e indexação de documentos PDF com ElasticSearch.**

O **ElasticPDF** é uma aplicação desenvolvida em **.NET** que utiliza o **Elasticsearch como mecanismo central de indexação e busca de documentos**.

A proposta do projeto é permitir que arquivos PDF sejam armazenados, processados e indexados, tornando seu conteúdo pesquisável de forma rápida e eficiente. A arquitetura utiliza processamento assíncrono para desacoplar o fluxo de upload do processamento e da indexação dos documentos.

![Status](https://img.shields.io/badge/status-in%20development-orange)

## 🏗️ Arquitetura
<img width="2131" height="950" alt="image" src="https://github.com/user-attachments/assets/7ae72693-e09e-4183-a053-ff3ab0362f45" />

### 🔍 ElasticSearch

O **Elasticsearch** é o componente principal do projeto.

Os documentos processados são indexados para permitir consultas rápidas e flexíveis, possibilitando evoluir posteriormente para funcionalidades como:

* 🔎 Busca por texto
* 🧠 Busca semântica
* 🏷️ Filtros e metadados
* 📊 Relevância dos resultados
* ⚡ Consultas em grandes volumes de documentos

A utilização do Elasticsearch permite que o sistema vá além de uma simples busca por nome de arquivo, transformando o conteúdo dos PDFs em uma fonte pesquisável.

### ⚙️ Componentes

| Componente         | Responsabilidade                   |
| ------------------ | ---------------------------------- |
| **Elasticsearch**  | Indexação e busca dos documentos   |
| **AspNet**         | API e regras da aplicação          |
| **RabbitMQ**       | Eventos e processamento assíncrono |
| **MinIO**          | Armazenamento dos arquivos PDF     |
| **Docker Compose** | Orquestração dos serviços          |

## 🚀 Executando

Com o Docker instalado:

```bash
docker compose up -d
```

**ElasticPDF** — *Turning PDFs into searchable data.*

![.NET](https://img.shields.io/badge/.NET-10-512BD4?logo=dotnet\&logoColor=white)
![Elasticsearch](https://img.shields.io/badge/Elasticsearch-9-005571?logo=elasticsearch\&logoColor=white)
![RabbitMQ](https://img.shields.io/badge/RabbitMQ-4-FF6600?logo=rabbitmq\&logoColor=white)
![MinIO](https://img.shields.io/badge/MinIO-Storage-C72E29?logo=minio\&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-Compose-2496ED?logo=docker\&logoColor=white)
![Architecture](https://img.shields.io/badge/architecture-event--driven-blue)
