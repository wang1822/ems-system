# EMS System Deployment Guide

## 部署指南

本文档提供 EMS 能源管理系统的完整部署说明。

## 系统架构

```
┌─────────────┐     ┌──────────────┐     ┌──────────┐
│   React     │────▶│  ASP.NET     │────▶│  MySQL   │
│  Frontend   │     │  Core API    │     │ Database │
│  (Port 3000)│     │  (Port 5000) │     │          │
└─────────────┘     └──────────────┘     └──────────┘
```

## 前置要求

### 软件要求

1. **.NET SDK 8.0+**
   - 下载: https://dotnet.microsoft.com/download
   
2. **MySQL 8.0+**
   - 下载: https://dev.mysql.com/downloads/
   
3. **Node.js 18+**
   - 下载: https://nodejs.org/

## 快速部署 (开发环境)

### 1. 克隆代码

```bash
git clone https://github.com/wang1822/ems-system.git
cd ems-system
```

### 2. 设置数据库

启动 MySQL 并创建数据库:

```bash
mysql -u root -p
```

```sql
CREATE DATABASE ems_db;
EXIT;
```

导入数据库架构:

```bash
mysql -u root -p ems_db < database/schema.sql
```

### 3. 配置后端

编辑 `backend/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=ems_db;User=root;Password=YOUR_PASSWORD;"
  },
  "Jwt": {
    "Key": "your-secret-key-min-32-chars-long-CHANGE-THIS!",
    "Issuer": "EMSBackend",
    "Audience": "EMSBackend",
    "ExpireMinutes": 60
  }
}
```

### 4. 启动后端

```bash
cd backend
dotnet restore
dotnet run
```

后端将在 http://localhost:5000 启动
Swagger UI: https://localhost:5001/swagger

### 5. 配置前端

```bash
cd frontend/ems-frontend
cp .env.example .env
```

编辑 `.env`:

```env
REACT_APP_API_URL=http://localhost:5000/api
```

### 6. 启动前端

```bash
cd frontend/ems-frontend
npm install
npm start
```

前端将在 http://localhost:3000 启动

## 生产环境部署

### 选项 1: 传统部署

#### 后端部署

1. **发布应用**

```bash
cd backend
dotnet publish -c Release -o ./publish
```

2. **配置生产环境**

编辑 `appsettings.Production.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_DB_HOST;Database=ems_db;User=YOUR_USER;Password=YOUR_PASSWORD;"
  },
  "Jwt": {
    "Key": "STRONG-SECRET-KEY-MIN-32-CHARS-PRODUCTION",
    "Issuer": "EMSBackend",
    "Audience": "EMSBackend",
    "ExpireMinutes": 60
  }
}
```

3. **运行应用**

```bash
cd publish
dotnet EMSBackend.dll --environment=Production
```

或使用系统服务 (systemd):

创建 `/etc/systemd/system/ems-backend.service`:

```ini
[Unit]
Description=EMS Backend API

[Service]
WorkingDirectory=/var/www/ems/backend
ExecStart=/usr/bin/dotnet /var/www/ems/backend/EMSBackend.dll
Restart=always
RestartSec=10
User=www-data
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false

[Install]
WantedBy=multi-user.target
```

启动服务:

```bash
sudo systemctl enable ems-backend
sudo systemctl start ems-backend
```

#### 前端部署

1. **构建应用**

```bash
cd frontend/ems-frontend
npm run build
```

2. **配置 Nginx**

创建 `/etc/nginx/sites-available/ems`:

```nginx
server {
    listen 80;
    server_name your-domain.com;
    
    # Frontend
    location / {
        root /var/www/ems/frontend;
        index index.html;
        try_files $uri $uri/ /index.html;
    }
    
    # Backend API
    location /api {
        proxy_pass http://localhost:5000;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection keep-alive;
        proxy_set_header Host $host;
        proxy_cache_bypass $http_upgrade;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
    }
}
```

启用站点:

```bash
sudo ln -s /etc/nginx/sites-available/ems /etc/nginx/sites-enabled/
sudo nginx -t
sudo systemctl reload nginx
```

### 选项 2: Docker 部署

#### 创建 Docker Compose 文件

创建 `docker-compose.yml`:

```yaml
version: '3.8'

services:
  mysql:
    image: mysql:8.0
    environment:
      MYSQL_ROOT_PASSWORD: root_password
      MYSQL_DATABASE: ems_db
      MYSQL_USER: ems_user
      MYSQL_PASSWORD: ems_password
    ports:
      - "3306:3306"
    volumes:
      - mysql_data:/var/lib/mysql
      - ./database/schema.sql:/docker-entrypoint-initdb.d/schema.sql

  backend:
    build: ./backend
    ports:
      - "5000:80"
    depends_on:
      - mysql
    environment:
      - ConnectionStrings__DefaultConnection=Server=mysql;Database=ems_db;User=ems_user;Password=ems_password;
      - Jwt__Key=YOUR-STRONG-SECRET-KEY-MIN-32-CHARS

  frontend:
    build: ./frontend/ems-frontend
    ports:
      - "80:80"
    depends_on:
      - backend
    environment:
      - REACT_APP_API_URL=http://backend/api

volumes:
  mysql_data:
```

#### 创建后端 Dockerfile

创建 `backend/Dockerfile`:

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY *.csproj ./
RUN dotnet restore
COPY . ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .
EXPOSE 80
ENTRYPOINT ["dotnet", "EMSBackend.dll"]
```

#### 创建前端 Dockerfile

创建 `frontend/ems-frontend/Dockerfile`:

```dockerfile
FROM node:18 AS build
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
RUN npm run build

FROM nginx:alpine
COPY --from=build /app/build /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
```

创建 `frontend/ems-frontend/nginx.conf`:

```nginx
server {
    listen 80;
    location / {
        root /usr/share/nginx/html;
        index index.html index.htm;
        try_files $uri $uri/ /index.html;
    }
}
```

#### 部署

```bash
docker-compose up -d
```

## 安全配置

### 1. 启用 HTTPS

使用 Let's Encrypt:

```bash
sudo apt install certbot python3-certbot-nginx
sudo certbot --nginx -d your-domain.com
```

### 2. 配置防火墙

```bash
sudo ufw allow 80/tcp
sudo ufw allow 443/tcp
sudo ufw allow 22/tcp
sudo ufw enable
```

### 3. 数据库安全

```bash
mysql_secure_installation
```

### 4. 定期备份

创建备份脚本 `/usr/local/bin/backup-ems.sh`:

```bash
#!/bin/bash
DATE=$(date +%Y%m%d_%H%M%S)
mysqldump -u root -p ems_db > /backup/ems_db_$DATE.sql
find /backup -name "ems_db_*.sql" -mtime +7 -delete
```

添加到 crontab:

```bash
0 2 * * * /usr/local/bin/backup-ems.sh
```

## 监控和日志

### 应用日志

后端日志位置: `/var/log/ems/backend.log`

前端访问日志: `/var/log/nginx/access.log`

### 健康检查

后端健康检查端点: `https://your-domain.com/api/health`

## 故障排除

### 数据库连接失败

检查连接字符串和数据库服务状态:

```bash
sudo systemctl status mysql
```

### 后端启动失败

查看日志:

```bash
sudo journalctl -u ems-backend -n 50
```

### 前端 API 调用失败

检查 CORS 配置和 API 地址

### 性能优化

1. 启用 MySQL 查询缓存
2. 配置 Nginx 缓存
3. 使用 CDN 分发静态资源
4. 启用 Gzip 压缩

## 维护

### 更新应用

```bash
# 备份数据库
mysqldump -u root -p ems_db > backup.sql

# 拉取最新代码
git pull

# 更新后端
cd backend
dotnet publish -c Release -o ./publish
sudo systemctl restart ems-backend

# 更新前端
cd frontend/ems-frontend
npm install
npm run build
sudo cp -r build/* /var/www/ems/frontend/
```

### 数据库迁移

使用 Entity Framework Core 迁移:

```bash
cd backend
dotnet ef migrations add MigrationName
dotnet ef database update
```

## 支持

如遇问题，请访问:
- GitHub Issues: https://github.com/wang1822/ems-system/issues
- 文档: https://github.com/wang1822/ems-system/blob/main/README.md
