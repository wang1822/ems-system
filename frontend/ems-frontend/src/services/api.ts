import axios, { AxiosInstance } from 'axios';

const API_BASE_URL = process.env.REACT_APP_API_URL || 'http://localhost:5000/api';

class ApiService {
  private api: AxiosInstance;

  constructor() {
    this.api = axios.create({
      baseURL: API_BASE_URL,
      headers: {
        'Content-Type': 'application/json',
      },
    });

    // Add request interceptor to include auth token
    this.api.interceptors.request.use(
      (config) => {
        const token = localStorage.getItem('token');
        if (token) {
          config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
      },
      (error) => {
        return Promise.reject(error);
      }
    );
  }

  // Auth
  async login(username: string, password: string) {
    const response = await this.api.post('/auth/login', { username, password });
    return response.data;
  }

  async register(data: any) {
    const response = await this.api.post('/auth/register', data);
    return response.data;
  }

  async getCurrentUser() {
    const response = await this.api.get('/auth/me');
    return response.data;
  }

  // Power Stations
  async getPowerStations(params?: any) {
    const response = await this.api.get('/powerstations', { params });
    return response.data;
  }

  async getPowerStation(id: number) {
    const response = await this.api.get(`/powerstations/${id}`);
    return response.data;
  }

  async createPowerStation(data: any) {
    const response = await this.api.post('/powerstations', data);
    return response.data;
  }

  async updatePowerStation(id: number, data: any) {
    const response = await this.api.put(`/powerstations/${id}`, data);
    return response.data;
  }

  async deletePowerStation(id: number) {
    await this.api.delete(`/powerstations/${id}`);
  }

  // Devices
  async getDevices(params?: any) {
    const response = await this.api.get('/devices', { params });
    return response.data;
  }

  async getDevice(id: number) {
    const response = await this.api.get(`/devices/${id}`);
    return response.data;
  }

  async createDevice(data: any) {
    const response = await this.api.post('/devices', data);
    return response.data;
  }

  async updateDevice(id: number, data: any) {
    const response = await this.api.put(`/devices/${id}`, data);
    return response.data;
  }

  async deleteDevice(id: number) {
    await this.api.delete(`/devices/${id}`);
  }

  // Monitoring
  async getMonitoringData(params?: any) {
    const response = await this.api.get('/monitoring', { params });
    return response.data;
  }

  async getLatestMonitoringData(deviceId: number) {
    const response = await this.api.get(`/monitoring/device/${deviceId}/latest`);
    return response.data;
  }

  async createMonitoringData(data: any) {
    const response = await this.api.post('/monitoring', data);
    return response.data;
  }

  // Events
  async getEvents(params?: any) {
    const response = await this.api.get('/events', { params });
    return response.data;
  }

  async getEvent(id: number) {
    const response = await this.api.get(`/events/${id}`);
    return response.data;
  }

  async createEvent(data: any) {
    const response = await this.api.post('/events', data);
    return response.data;
  }

  async resolveEvent(id: number, resolvedBy?: string) {
    const response = await this.api.put(`/events/${id}/resolve`, resolvedBy);
    return response.data;
  }

  // Factory Tests
  async getFactoryTests(params?: any) {
    const response = await this.api.get('/factorytests', { params });
    return response.data;
  }

  async getFactoryTest(id: number) {
    const response = await this.api.get(`/factorytests/${id}`);
    return response.data;
  }

  async createFactoryTest(data: any) {
    const response = await this.api.post('/factorytests', data);
    return response.data;
  }

  async updateFactoryTest(id: number, data: any) {
    const response = await this.api.put(`/factorytests/${id}`, data);
    return response.data;
  }

  async deleteFactoryTest(id: number) {
    await this.api.delete(`/factorytests/${id}`);
  }

  // Construction
  async getConstructionProcesses(params?: any) {
    const response = await this.api.get('/construction', { params });
    return response.data;
  }

  async getConstructionProcess(id: number) {
    const response = await this.api.get(`/construction/${id}`);
    return response.data;
  }

  async createConstructionProcess(data: any) {
    const response = await this.api.post('/construction', data);
    return response.data;
  }

  async updateConstructionProcess(id: number, data: any) {
    const response = await this.api.put(`/construction/${id}`, data);
    return response.data;
  }

  async deleteConstructionProcess(id: number) {
    await this.api.delete(`/construction/${id}`);
  }

  // Statistics
  async getStatistics(params?: any) {
    const response = await this.api.get('/statistics', { params });
    return response.data;
  }

  async getStatisticsSummary(params?: any) {
    const response = await this.api.get('/statistics/summary', { params });
    return response.data;
  }

  async createStatistics(data: any) {
    const response = await this.api.post('/statistics', data);
    return response.data;
  }
}

const apiService = new ApiService();
export default apiService;
