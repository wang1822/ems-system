# EMS System - 能源管理系统

这是一个模块化的能源管理系统 (Energy Management System)，使用 C# + ASP.NET Core 8.0 作为后端，React 作为前端，MySQL 作为数据库。

## 系统功能模块

### 1. 电站管理模块 (Power Station Management)
- 管理电站信息
- 支持增删改查操作
- 包括电站位置及其元数据的管理

### 2. 设备监控模块 (Device Monitoring)
- 实时监控设备状态
- 显示电池电量、温度、运行状态等关键指标
- 支持历史数据查询

### 3. 设备信息模块 (Device Information)
- 管理设备和组件的静态信息
- 包括制造商、设备规格等元数据

### 4. 事件查询模块 (Event Query)
- 查询历史事件
- 包括警告、错误和维护日志等
- 支持事件筛选和解决状态管理

### 5. 出厂检测模块 (Factory Testing)
- 自动化验证设备是否符合部署标准
- 确保设备在生产中可以正常使用
- 记录测试结果

### 6. 建站流程模块 (Construction Process)
- 管理设置电站过程中涉及的关键步骤
- 如建设批准、安装跟踪等
- 支持多阶段流程管理

### 7. 数据统计模块 (Data Statistics)
- 生成关键数据的报告
- 包括系统运行时间、充放电统计、告警统计等

## 技术栈

### 后端 (Backend)
- **语言**: C# 
- **框架**: ASP.NET Core 8.0
- **API**: RESTful API
- **数据库**: MySQL 8.0+
- **ORM**: Entity Framework Core
- **认证**: JWT Bearer Authentication
- **密码加密**: BCrypt.Net

### 前端 (Frontend)
- **框架**: React (计划实现)
- **语言**: JavaScript/TypeScript

### 数据库 (Database)
- **类型**: MySQL 8.0+
- **特性**: 关系型数据存储，支持事务

## 快速开始

### 前置要求

1. **.NET SDK 8.0 或更高版本**
   ```bash
   dotnet --version
   ```

2. **MySQL 8.0 或更高版本**
   ```bash
   mysql --version
   ```

3. **Node.js 18+ 和 npm (用于前端，可选)**
   ```bash
   node --version
   npm --version
   ```

### 后端设置

1. **克隆仓库**
   ```bash
   git clone https://github.com/wang1822/ems-system.git
   cd ems-system
   ```

2. **配置数据库**
   
   创建 MySQL 数据库：
   ```bash
   mysql -u root -p < database/schema.sql
   ```
   
   或手动创建：
   ```sql
   CREATE DATABASE ems_db;
   ```

3. **配置连接字符串**
   
   编辑 `backend/appsettings.json`：
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=ems_db;User=root;Password=your_password;"
     },
     "Jwt": {
       "Key": "your-secret-key-min-32-chars-long-change-in-production!",
       "Issuer": "EMSBackend",
       "Audience": "EMSBackend",
       "ExpireMinutes": 60
     }
   }
   ```

4. **安装依赖并运行**
   ```bash
   cd backend
   dotnet restore
   dotnet build
   dotnet run
   ```

5. **访问 API 文档**
   
   应用启动后，访问 Swagger UI：
   ```
   https://localhost:5001/swagger
   ```

### 数据库迁移 (可选)

使用 Entity Framework Core 迁移：

```bash
cd backend
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## API 端点

### 认证 (Authentication)
- `POST /api/auth/register` - 注册新用户
- `POST /api/auth/login` - 用户登录
- `GET /api/auth/me` - 获取当前用户信息

### 电站管理 (Power Stations)
- `GET /api/powerstations` - 获取所有电站
- `GET /api/powerstations/{id}` - 获取指定电站
- `POST /api/powerstations` - 创建新电站
- `PUT /api/powerstations/{id}` - 更新电站
- `DELETE /api/powerstations/{id}` - 删除电站

### 设备管理 (Devices)
- `GET /api/devices` - 获取所有设备
- `GET /api/devices/{id}` - 获取指定设备
- `POST /api/devices` - 创建新设备
- `PUT /api/devices/{id}` - 更新设备
- `DELETE /api/devices/{id}` - 删除设备

### 设备监控 (Monitoring)
- `GET /api/monitoring` - 获取监控数据
- `GET /api/monitoring/device/{deviceId}/latest` - 获取设备最新监控数据
- `POST /api/monitoring` - 创建监控数据

### 事件管理 (Events)
- `GET /api/events` - 获取所有事件
- `GET /api/events/{id}` - 获取指定事件
- `POST /api/events` - 创建新事件
- `PUT /api/events/{id}/resolve` - 解决事件

### 出厂检测 (Factory Tests)
- `GET /api/factorytests` - 获取所有测试
- `GET /api/factorytests/{id}` - 获取指定测试
- `POST /api/factorytests` - 创建新测试
- `PUT /api/factorytests/{id}` - 更新测试
- `DELETE /api/factorytests/{id}` - 删除测试

### 建站流程 (Construction)
- `GET /api/construction` - 获取所有建站流程
- `GET /api/construction/{id}` - 获取指定流程
- `POST /api/construction` - 创建新流程
- `PUT /api/construction/{id}` - 更新流程
- `DELETE /api/construction/{id}` - 删除流程

### 数据统计 (Statistics)
- `GET /api/statistics` - 获取统计数据
- `GET /api/statistics/summary` - 获取统计摘要
- `POST /api/statistics` - 创建统计记录

## 开发指南

### 项目结构

```
ems-system/
├── backend/                 # ASP.NET Core 后端
│   ├── Controllers/        # API 控制器
│   ├── Models/            # 数据模型
│   ├── DTOs/              # 数据传输对象
│   ├── Data/              # 数据库上下文
│   ├── Services/          # 业务逻辑服务
│   ├── Program.cs         # 应用入口
│   └── appsettings.json   # 配置文件
├── frontend/              # React 前端 (待实现)
├── database/              # 数据库脚本
│   └── schema.sql         # 数据库架构
└── README.md             # 项目文档
```

### 添加新模块

1. 在 `Models/` 中创建数据模型
2. 在 `DTOs/` 中创建对应的 DTO
3. 在 `Controllers/` 中创建控制器
4. 更新 `EMSDbContext.cs` 添加 DbSet
5. 创建数据库迁移

### 代码规范

- 使用 Pascal Case 命名类和方法
- 使用 Camel Case 命名变量和参数
- 使用异步方法 (async/await)
- 遵循 RESTful API 设计原则
- 所有 API 端点需要身份验证（除了 /auth/register 和 /auth/login）

## 安全性

- 使用 JWT Token 进行身份验证
- 密码使用 BCrypt 加密
- CORS 配置支持跨域请求
- 所有 API 端点默认需要授权

## 测试

```bash
cd backend
dotnet test
```

## 部署

### 生产环境配置

1. 更新 `appsettings.Production.json`
2. 设置强密码和密钥
3. 配置 HTTPS
4. 设置环境变量

### Docker 部署 (待完成)

```bash
docker-compose up -d
```

## 贡献

欢迎提交 Pull Request 或报告 Issue。

## 许可证

本项目采用 MIT 许可证。

## 联系方式

如有问题，请联系项目维护者或提交 Issue。
