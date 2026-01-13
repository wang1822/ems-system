import React, { useCallback, useEffect, useMemo, useState } from 'react';
import { useAuth } from '../context/AuthContext';
import { Link } from 'react-router-dom';
import api from '../services/api';
import {
  Area,
  AreaChart,
  Bar,
  BarChart,
  Cell,
  CartesianGrid,
  Legend,
  Line,
  LineChart,
  Pie,
  PieChart,
  ResponsiveContainer,
  Tooltip,
  XAxis,
  YAxis,
} from 'recharts';
import './Dashboard.css';

interface StatsSummary {
  energyGeneratedTotal: number;
  energyConsumedTotal: number;
  totalEvents: number;
  unresolvedEvents: number;
  period: string;
}

interface ModuleChartData {
  powerStations: { name: string; capacity: number; generation: number; utilization: number }[];
  deviceStatuses: { type: string; online: number; maintenance: number; offline: number }[];
  monitoringTrend: { label: string; generated: number; consumed: number; temperature: number }[];
  eventSummary: { category: string; count: number }[];
  factoryTests: { status: string; value: number }[];
  constructionProgress: { phase: string; progress: number; delayRisk: number }[];
  statisticTrend: { month: string; uptime: number; charge: number; discharge: number }[];
  generatedAt: string;
  summary: StatsSummary;
}

const createDemoData = (): ModuleChartData => {
  const powerStations = [
    { name: '北部能源站', capacity: 5200, generation: 4680, utilization: 90 },
    { name: '华东光伏场', capacity: 3800, generation: 3240, utilization: 85 },
    { name: '西南储能站', capacity: 2600, generation: 2120, utilization: 82 },
    { name: '华南微网', capacity: 1800, generation: 1460, utilization: 81 },
  ];

  const deviceStatuses = [
    { type: '储能电池', online: 18, maintenance: 2, offline: 1 },
    { type: '逆变器', online: 14, maintenance: 1, offline: 1 },
    { type: '气象站', online: 9, maintenance: 1, offline: 0 },
    { type: '监测网关', online: 11, maintenance: 1, offline: 1 },
  ];

  const monitoringTrend = Array.from({ length: 7 }).map((_, idx) => {
    const generated = 420 + idx * 36 + (idx % 2 === 0 ? 20 : 8);
    const consumed = 170 + idx * 14 + (idx % 3 === 0 ? 12 : 0);
    return {
      label: `周${idx + 1}`,
      generated,
      consumed,
      temperature: 24 + idx * 0.8 + (idx % 2 === 0 ? 1.2 : 0),
    };
  });

  const eventSummary = [
    { category: 'warning', count: 12 },
    { category: 'error', count: 5 },
    { category: 'maintenance', count: 7 },
  ];

  const factoryTests = [
    { status: '通过', value: 28 },
    { status: '进行中', value: 4 },
    { status: '待执行', value: 3 },
    { status: '失败', value: 2 },
  ];

  const constructionProgress = [
    { phase: '勘察', progress: 100, delayRisk: 2 },
    { phase: '设计', progress: 92, delayRisk: 4 },
    { phase: '并网', progress: 78, delayRisk: 6 },
    { phase: '调试', progress: 64, delayRisk: 9 },
    { phase: '验收', progress: 42, delayRisk: 12 },
  ];

  const statisticTrend = [
    { month: '1月', uptime: 98.5, charge: 620, discharge: 580 },
    { month: '2月', uptime: 97.8, charge: 640, discharge: 605 },
    { month: '3月', uptime: 98.9, charge: 680, discharge: 642 },
    { month: '4月', uptime: 99.1, charge: 710, discharge: 688 },
    { month: '5月', uptime: 98.7, charge: 735, discharge: 704 },
    { month: '6月', uptime: 99.3, charge: 760, discharge: 732 },
  ];

  const summary: StatsSummary = {
    energyGeneratedTotal: monitoringTrend.reduce((sum, d) => sum + d.generated, 0),
    energyConsumedTotal: monitoringTrend.reduce((sum, d) => sum + d.consumed, 0),
    totalEvents: eventSummary.reduce((sum, d) => sum + d.count, 0),
    unresolvedEvents: eventSummary
      .filter((d) => d.category !== 'maintenance')
      .reduce((sum, d) => sum + Math.max(1, Math.floor(d.count * 0.6)), 0),
    period: 'demo',
  };

  return {
    powerStations,
    deviceStatuses,
    monitoringTrend,
    eventSummary,
    factoryTests,
    constructionProgress,
    statisticTrend,
    generatedAt: new Date().toLocaleString('zh-CN', { hour12: false }),
    summary,
  };
};

const Dashboard: React.FC = () => {
  const { user, logout } = useAuth();
  const demoData = useMemo(createDemoData, []);
  const [stats, setStats] = useState<StatsSummary | null>(null);
  const [loading, setLoading] = useState(true);

  const fetchStats = useCallback(async () => {
    try {
      const data = await api.getStatisticsSummary();
      setStats(data);
    } catch (error) {
      console.error('Failed to fetch statistics:', error);
      setStats(demoData.summary);
    } finally {
      setLoading(false);
    }
  }, [demoData.summary]);

  useEffect(() => {
    fetchStats();
  }, [fetchStats]);

  return (
    <div className="dashboard">
      <nav className="dashboard-nav">
        <h1>EMS 能源管理系统</h1>
        <div className="nav-user">
          <span>欢迎, {user?.fullName || user?.username}</span>
          <button onClick={logout} className="logout-button">退出</button>
        </div>
      </nav>

      <div className="dashboard-content">
        <h2>仪表板</h2>
        
        {loading ? (
          <p>加载中...</p>
        ) : stats && (
          <div className="stats-grid">
            <div className="stat-card">
              <h3>总发电量</h3>
              <p className="stat-value">{stats.energyGeneratedTotal.toFixed(2)} kWh</p>
            </div>
            <div className="stat-card">
              <h3>总耗电量</h3>
              <p className="stat-value">{stats.energyConsumedTotal.toFixed(2)} kWh</p>
            </div>
            <div className="stat-card">
              <h3>总事件数</h3>
              <p className="stat-value">{stats.totalEvents}</p>
            </div>
            <div className="stat-card">
              <h3>未解决事件</h3>
            <p className="stat-value alert">{stats.unresolvedEvents}</p>
          </div>
        </div>
      )}

      <div className="module-charts-header">
        <div>
          <h2>模块图表</h2>
          <p className="module-charts-subtitle">基于演示数据实时生成，每个模块的核心指标一目了然</p>
        </div>
        <div className="module-charts-badge">数据生成时间：{demoData.generatedAt}</div>
      </div>
      <div className="charts-grid">
        <div className="chart-card">
          <div className="chart-card-header">
            <h3>电站管理 · 容量与发电</h3>
            <span>容量/发电量 (MWh)</span>
          </div>
          <ResponsiveContainer width="100%" height={240}>
            <BarChart data={demoData.powerStations}>
              <CartesianGrid strokeDasharray="3 3" vertical={false} />
              <XAxis dataKey="name" />
              <YAxis />
              <Tooltip />
              <Legend />
              <Bar dataKey="capacity" name="装机容量" fill="#a5b4fc" radius={[4, 4, 0, 0]} />
              <Bar dataKey="generation" name="月发电量" fill="#667eea" radius={[4, 4, 0, 0]} />
            </BarChart>
          </ResponsiveContainer>
        </div>

        <div className="chart-card">
          <div className="chart-card-header">
            <h3>设备信息 · 状态分布</h3>
            <span>在线/维护/离线 (台)</span>
          </div>
          <ResponsiveContainer width="100%" height={240}>
            <BarChart data={demoData.deviceStatuses} stackOffset="sign">
              <CartesianGrid strokeDasharray="3 3" vertical={false} />
              <XAxis dataKey="type" />
              <YAxis />
              <Tooltip />
              <Legend />
              <Bar dataKey="online" name="在线" stackId="status" fill="#34d399" radius={[4, 4, 0, 0]} />
              <Bar dataKey="maintenance" name="维护" stackId="status" fill="#fbbf24" radius={[4, 4, 0, 0]} />
              <Bar dataKey="offline" name="离线" stackId="status" fill="#f87171" radius={[4, 4, 0, 0]} />
            </BarChart>
          </ResponsiveContainer>
        </div>

        <div className="chart-card">
          <div className="chart-card-header">
            <h3>设备监控 · 能源趋势</h3>
            <span>周度发电/耗电 (kWh)</span>
          </div>
          <ResponsiveContainer width="100%" height={240}>
            <AreaChart data={demoData.monitoringTrend}>
              <CartesianGrid strokeDasharray="3 3" />
              <XAxis dataKey="label" />
              <YAxis />
              <Tooltip />
              <Legend />
              <Area type="monotone" dataKey="generated" name="发电量" stroke="#60a5fa" fill="#bfdbfe" />
              <Area type="monotone" dataKey="consumed" name="耗电量" stroke="#f472b6" fill="#fce7f3" />
            </AreaChart>
          </ResponsiveContainer>
        </div>

        <div className="chart-card">
          <div className="chart-card-header">
            <h3>事件查询 · 告警概览</h3>
            <span>事件数量</span>
          </div>
          <ResponsiveContainer width="100%" height={240}>
            <BarChart data={demoData.eventSummary}>
              <CartesianGrid strokeDasharray="3 3" vertical={false} />
              <XAxis dataKey="category" tickFormatter={(value) => value.toUpperCase()} />
              <YAxis allowDecimals={false} />
              <Tooltip />
              <Legend />
              <Bar dataKey="count" name="事件数" fill="#fb7185" radius={[4, 4, 0, 0]} />
            </BarChart>
          </ResponsiveContainer>
        </div>

        <div className="chart-card">
          <div className="chart-card-header">
            <h3>出厂检测 · 状态</h3>
            <span>检测批次</span>
          </div>
          <ResponsiveContainer width="100%" height={240}>
            <PieChart>
              <Pie
                data={demoData.factoryTests}
                dataKey="value"
                nameKey="status"
                cx="50%"
                cy="50%"
                outerRadius={90}
                label
              >
                {demoData.factoryTests.map((_, index) => (
                  <Cell
                    key={`cell-${index}`}
                    fill={['#34d399', '#60a5fa', '#fbbf24', '#f87171'][index % 4]}
                  />
                ))}
              </Pie>
              <Tooltip />
              <Legend />
            </PieChart>
          </ResponsiveContainer>
        </div>

        <div className="chart-card">
          <div className="chart-card-header">
            <h3>建站流程 · 进度</h3>
            <span>阶段完成率</span>
          </div>
          <ResponsiveContainer width="100%" height={240}>
            <LineChart data={demoData.constructionProgress}>
              <CartesianGrid strokeDasharray="3 3" />
              <XAxis dataKey="phase" />
              <YAxis domain={[0, 110]} />
              <Tooltip />
              <Legend />
              <Line type="monotone" dataKey="progress" name="完成度%" stroke="#a855f7" strokeWidth={2} />
              <Line type="monotone" dataKey="delayRisk" name="延期风险%" stroke="#f97316" strokeDasharray="5 5" />
            </LineChart>
          </ResponsiveContainer>
        </div>

        <div className="chart-card">
          <div className="chart-card-header">
            <h3>数据统计 · 效率</h3>
            <span>月度趋势</span>
          </div>
          <ResponsiveContainer width="100%" height={240}>
            <LineChart data={demoData.statisticTrend}>
              <CartesianGrid strokeDasharray="3 3" />
              <XAxis dataKey="month" />
              <YAxis />
              <Tooltip />
              <Legend />
              <Line type="monotone" dataKey="uptime" name="设备可用率 %" stroke="#10b981" strokeWidth={2} />
              <Line type="monotone" dataKey="charge" name="充电量 (MWh)" stroke="#3b82f6" />
              <Line type="monotone" dataKey="discharge" name="放电量 (MWh)" stroke="#ec4899" />
            </LineChart>
          </ResponsiveContainer>
        </div>
      </div>

      <h2>功能模块</h2>
      <div className="modules-grid">
        <Link to="/power-stations" className="module-card">
          <h3>🏭 电站管理</h3>
          <p>管理电站信息，位置及元数据</p>
          </Link>

          <Link to="/devices" className="module-card">
            <h3>📱 设备信息</h3>
            <p>管理设备和组件静态信息</p>
          </Link>

          <Link to="/monitoring" className="module-card">
            <h3>📊 设备监控</h3>
            <p>实时监控设备状态和关键指标</p>
          </Link>

          <Link to="/events" className="module-card">
            <h3>⚠️ 事件查询</h3>
            <p>查询警告、错误和维护日志</p>
          </Link>

          <Link to="/factory-tests" className="module-card">
            <h3>🔬 出厂检测</h3>
            <p>验证设备是否符合部署标准</p>
          </Link>

          <Link to="/construction" className="module-card">
            <h3>🏗️ 建站流程</h3>
            <p>管理建设批准、安装跟踪等</p>
          </Link>

          <Link to="/statistics" className="module-card">
            <h3>📈 数据统计</h3>
            <p>系统运行时间、充放电统计等</p>
          </Link>
        </div>
      </div>
    </div>
  );
};

export default Dashboard;
