# EMS API Documentation

## 基本信息

- **Base URL**: `http://localhost:5000/api`
- **认证方式**: JWT Bearer Token
- **内容类型**: `application/json`

## 认证

所有 API 端点（除了注册和登录）都需要在请求头中包含有效的 JWT token:

```
Authorization: Bearer <your_jwt_token>
```

## API 端点

### 认证模块 (Authentication)

#### 注册用户
```
POST /api/auth/register
```

**请求体**:
```json
{
  "username": "string",
  "email": "string",
  "password": "string",
  "fullName": "string (optional)"
}
```

**响应**: `201 Created`
```json
{
  "id": 1,
  "username": "admin",
  "email": "admin@example.com",
  "fullName": "管理员",
  "isActive": true,
  "isSuperuser": false,
  "createdAt": "2024-01-01T00:00:00Z"
}
```

#### 用户登录
```
POST /api/auth/login
```

**请求体**:
```json
{
  "username": "string",
  "password": "string"
}
```

**响应**: `200 OK`
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "tokenType": "Bearer",
  "user": {
    "id": 1,
    "username": "admin",
    "email": "admin@example.com",
    "fullName": "管理员",
    "isActive": true,
    "isSuperuser": false,
    "createdAt": "2024-01-01T00:00:00Z"
  }
}
```

#### 获取当前用户信息
```
GET /api/auth/me
```

**响应**: `200 OK`
```json
{
  "id": 1,
  "username": "admin",
  "email": "admin@example.com",
  "fullName": "管理员",
  "isActive": true,
  "isSuperuser": false,
  "createdAt": "2024-01-01T00:00:00Z"
}
```

---

### 电站管理模块 (Power Stations)

#### 获取所有电站
```
GET /api/powerstations?skip=0&limit=100
```

**查询参数**:
- `skip`: 跳过的记录数 (默认: 0)
- `limit`: 返回的最大记录数 (默认: 100)

**响应**: `200 OK`
```json
[
  {
    "id": 1,
    "name": "北京第一电站",
    "location": "北京市朝阳区",
    "latitude": 39.9042,
    "longitude": 116.4074,
    "capacity": 5000.0,
    "status": "active",
    "description": "主要电站",
    "createdAt": "2024-01-01T00:00:00Z",
    "updatedAt": "2024-01-01T00:00:00Z"
  }
]
```

#### 获取指定电站
```
GET /api/powerstations/{id}
```

**响应**: `200 OK` - 电站对象

#### 创建电站
```
POST /api/powerstations
```

**请求体**:
```json
{
  "name": "string",
  "location": "string",
  "latitude": 0.0,
  "longitude": 0.0,
  "capacity": 0.0,
  "status": "active",
  "description": "string"
}
```

**响应**: `201 Created` - 新建的电站对象

#### 更新电站
```
PUT /api/powerstations/{id}
```

**请求体**: 同创建电站

**响应**: `200 OK` - 更新后的电站对象

#### 删除电站
```
DELETE /api/powerstations/{id}
```

**响应**: `204 No Content`

---

### 设备管理模块 (Devices)

#### 获取所有设备
```
GET /api/devices?skip=0&limit=100&powerStationId=1
```

**查询参数**:
- `skip`: 跳过的记录数
- `limit`: 返回的最大记录数
- `powerStationId`: 按电站ID筛选 (可选)

**响应**: `200 OK`
```json
[
  {
    "id": 1,
    "powerStationId": 1,
    "name": "电池组A1",
    "deviceType": "battery",
    "manufacturer": "比亚迪",
    "model": "BYD-B100",
    "serialNumber": "SN123456",
    "specifications": "{\"capacity\": \"100kWh\"}",
    "installationDate": "2024-01-01T00:00:00Z",
    "warrantyExpiry": "2029-01-01T00:00:00Z",
    "status": "online",
    "createdAt": "2024-01-01T00:00:00Z",
    "updatedAt": "2024-01-01T00:00:00Z"
  }
]
```

#### 创建设备
```
POST /api/devices
```

**请求体**:
```json
{
  "powerStationId": 1,
  "name": "string",
  "deviceType": "string",
  "manufacturer": "string",
  "model": "string",
  "serialNumber": "string",
  "specifications": "string",
  "installationDate": "2024-01-01T00:00:00Z",
  "warrantyExpiry": "2029-01-01T00:00:00Z"
}
```

---

### 设备监控模块 (Monitoring)

#### 获取监控数据
```
GET /api/monitoring?deviceId=1&skip=0&limit=100
```

**响应**: `200 OK`
```json
[
  {
    "id": 1,
    "deviceId": 1,
    "batteryLevel": 85.5,
    "temperature": 25.3,
    "voltage": 48.2,
    "current": 120.5,
    "powerOutput": 5800.0,
    "energyGenerated": 1500.5,
    "energyConsumed": 100.2,
    "status": "normal",
    "errorCode": null,
    "recordedAt": "2024-01-01T12:00:00Z"
  }
]
```

#### 获取设备最新监控数据
```
GET /api/monitoring/device/{deviceId}/latest
```

#### 创建监控数据
```
POST /api/monitoring
```

**请求体**:
```json
{
  "deviceId": 1,
  "batteryLevel": 85.5,
  "temperature": 25.3,
  "voltage": 48.2,
  "current": 120.5,
  "powerOutput": 5800.0,
  "energyGenerated": 1500.5,
  "energyConsumed": 100.2,
  "status": "normal",
  "errorCode": null
}
```

---

### 事件管理模块 (Events)

#### 获取所有事件
```
GET /api/events?eventType=error&resolved=false&skip=0&limit=100
```

**查询参数**:
- `eventType`: 事件类型 (warning, error, maintenance, info)
- `deviceId`: 按设备ID筛选
- `powerStationId`: 按电站ID筛选
- `resolved`: 按解决状态筛选 (true/false)

**响应**: `200 OK`
```json
[
  {
    "id": 1,
    "deviceId": 1,
    "powerStationId": 1,
    "eventType": "error",
    "severity": "high",
    "title": "设备温度过高",
    "description": "电池组温度超过50°C",
    "resolved": false,
    "resolvedAt": null,
    "resolvedBy": null,
    "createdAt": "2024-01-01T12:00:00Z"
  }
]
```

#### 创建事件
```
POST /api/events
```

#### 解决事件
```
PUT /api/events/{id}/resolve
```

**请求体**:
```json
"解决人员姓名"
```

---

### 出厂检测模块 (Factory Tests)

#### 获取所有测试
```
GET /api/factorytests?deviceId=1&status=pending
```

**查询参数**:
- `deviceId`: 按设备ID筛选
- `status`: 按状态筛选 (pending, in_progress, passed, failed)

**响应**: `200 OK`
```json
[
  {
    "id": 1,
    "deviceId": 1,
    "testName": "电气性能测试",
    "testType": "electrical",
    "status": "passed",
    "result": "{\"voltage\": \"pass\", \"current\": \"pass\"}",
    "testedBy": "张三",
    "testedAt": "2024-01-01T10:00:00Z",
    "notes": "所有项目通过",
    "createdAt": "2024-01-01T09:00:00Z",
    "updatedAt": "2024-01-01T10:00:00Z"
  }
]
```

#### 创建测试
```
POST /api/factorytests
```

#### 更新测试
```
PUT /api/factorytests/{id}
```

---

### 建站流程模块 (Construction)

#### 获取所有流程
```
GET /api/construction?powerStationId=1&status=in_progress
```

**响应**: `200 OK`
```json
[
  {
    "id": 1,
    "powerStationId": 1,
    "phase": "site_survey",
    "status": "completed",
    "startDate": "2024-01-01T00:00:00Z",
    "endDate": "2024-01-05T00:00:00Z",
    "expectedCompletion": "2024-01-05T00:00:00Z",
    "responsiblePerson": "李四",
    "description": "现场勘察完成",
    "documents": "[\"doc1.pdf\", \"doc2.pdf\"]",
    "createdAt": "2024-01-01T00:00:00Z",
    "updatedAt": "2024-01-05T00:00:00Z"
  }
]
```

#### 创建流程
```
POST /api/construction
```

#### 更新流程
```
PUT /api/construction/{id}
```

---

### 数据统计模块 (Statistics)

#### 获取统计数据
```
GET /api/statistics?powerStationId=1&statisticType=uptime
```

**响应**: `200 OK`
```json
[
  {
    "id": 1,
    "powerStationId": 1,
    "statisticType": "uptime",
    "period": "daily",
    "periodStart": "2024-01-01T00:00:00Z",
    "periodEnd": "2024-01-01T23:59:59Z",
    "value": 98.5,
    "unit": "percentage",
    "data": "{\"details\": \"...}\",
    "createdAt": "2024-01-02T00:00:00Z"
  }
]
```

#### 获取统计摘要
```
GET /api/statistics/summary?powerStationId=1
```

**响应**: `200 OK`
```json
{
  "energyGeneratedTotal": 15000.5,
  "energyConsumedTotal": 1200.3,
  "totalEvents": 45,
  "unresolvedEvents": 3,
  "period": "all_time"
}
```

#### 创建统计记录
```
POST /api/statistics
```

---

## 错误响应

### 400 Bad Request
```json
{
  "message": "Invalid request data"
}
```

### 401 Unauthorized
```json
{
  "message": "Unauthorized"
}
```

### 404 Not Found
```json
{
  "message": "Resource not found"
}
```

### 500 Internal Server Error
```json
{
  "message": "Internal server error"
}
```

## 速率限制

目前没有速率限制，但建议客户端实施合理的请求频率控制。

## 分页

大多数列表端点支持分页:
- `skip`: 跳过的记录数
- `limit`: 返回的最大记录数 (最大 100)

## 时间格式

所有时间使用 ISO 8601 格式: `2024-01-01T12:00:00Z`

## 测试 API

使用 Swagger UI 测试 API: https://localhost:5001/swagger
