import React, { useEffect, useState } from 'react';
import { useAuth } from '../context/AuthContext';
import { Link } from 'react-router-dom';
import api from '../services/api';
import './Dashboard.css';

interface StatsSummary {
  energyGeneratedTotal: number;
  energyConsumedTotal: number;
  totalEvents: number;
  unresolvedEvents: number;
  period: string;
}

const Dashboard: React.FC = () => {
  const { user, logout } = useAuth();
  const [stats, setStats] = useState<StatsSummary | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    fetchStats();
  }, []);

  const fetchStats = async () => {
    try {
      const data = await api.getStatisticsSummary();
      setStats(data);
    } catch (error) {
      console.error('Failed to fetch statistics:', error);
    } finally {
      setLoading(false);
    }
  };

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
