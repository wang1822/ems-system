# EMS Frontend - React Application

这是 EMS 能源管理系统的前端应用，使用 React + TypeScript 构建。

## 功能特性

- 🔐 用户认证 (JWT)
- 📊 实时数据仪表板
- 🏭 电站管理
- 📱 设备管理
- 📈 设备监控
- ⚠️ 事件管理
- 🔬 出厂检测
- 🏗️ 建站流程
- 📊 数据统计

## 技术栈

- React 18
- TypeScript
- React Router v6
- Axios
- Recharts (数据可视化)

## 安装和运行

### 前置要求

- Node.js 18+
- npm 或 yarn

### 安装依赖

```bash
npm install
```

### 配置

复制 `.env.example` 为 `.env` 并配置：

```bash
cp .env.example .env
```

编辑 `.env` 文件：

```env
REACT_APP_API_URL=http://localhost:5000/api
REACT_APP_DEMO_MODE=true # 无后端环境时启用本地演示数据和图表
```

### 开发模式运行

```bash
npm start
```

应用将在 http://localhost:3000 上运行

### 构建生产版本

```bash
npm run build
```

构建产物将在 `build/` 目录中

## 项目结构

```
ems-frontend/
├── public/              # 静态资源
├── src/
│   ├── components/      # 可复用组件
│   ├── context/         # React Context (状态管理)
│   ├── pages/           # 页面组件
│   ├── services/        # API 服务
│   ├── App.tsx          # 主应用组件
│   └── index.tsx        # 应用入口
├── .env                 # 环境变量
├── package.json         # 依赖配置
└── tsconfig.json        # TypeScript 配置
```

## API 集成

应用通过 `src/services/api.ts` 与后端通信。所有 API 调用都会自动附加 JWT token。

## 许可证

MIT
