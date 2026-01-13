import React, { createContext, useContext, useState, useEffect, useCallback } from 'react';
import api from '../services/api';

interface User {
  id: number;
  username: string;
  email: string;
  fullName?: string;
  isActive: boolean;
  isSuperuser: boolean;
}

interface AuthContextType {
  user: User | null;
  token: string | null;
  login: (username: string, password: string) => Promise<void>;
  register: (data: any) => Promise<void>;
  logout: () => void;
  isAuthenticated: boolean;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const demoMode = process.env.REACT_APP_DEMO_MODE === 'true';
  const storedUser = localStorage.getItem('user');
  const [user, setUser] = useState<User | null>(storedUser ? JSON.parse(storedUser) : null);
  const [token, setToken] = useState<string | null>(localStorage.getItem('token'));

  const fetchCurrentUser = useCallback(async () => {
    if (demoMode) {
      return;
    }
    try {
      const userData = await api.getCurrentUser();
      setUser(userData);
    } catch (error) {
      console.error('Failed to fetch current user:', error);
      logout();
    }
  }, [demoMode]);

  useEffect(() => {
    if (demoMode) {
      const fallbackUser: User = storedUser
        ? JSON.parse(storedUser)
        : {
            id: 0,
            username: 'demo',
            email: 'demo@example.com',
            fullName: '演示账号',
            isActive: true,
            isSuperuser: false,
          };
      setUser(fallbackUser);
      setToken((existing) => {
        const value = existing ?? 'demo-token';
        localStorage.setItem('token', value);
        localStorage.setItem('user', JSON.stringify(fallbackUser));
        return value;
      });
      return;
    }

    if (token) {
      fetchCurrentUser();
    }
  }, [token, fetchCurrentUser, demoMode, storedUser]);

  const login = async (username: string, password: string) => {
    try {
      const response = await api.login(username, password);
      setToken(response.token);
      setUser(response.user);
      localStorage.setItem('token', response.token);
      localStorage.setItem('user', JSON.stringify(response.user));
    } catch (error) {
      console.error('Login failed:', error);
      throw error;
    }
  };

  const register = async (data: any) => {
    try {
      await api.register(data);
      // After registration, automatically login
      await login(data.username, data.password);
    } catch (error) {
      console.error('Registration failed:', error);
      throw error;
    }
  };

  const logout = () => {
    setToken(null);
    setUser(null);
    localStorage.removeItem('token');
    localStorage.removeItem('user');
  };

  const value: AuthContextType = {
    user,
    token,
    login,
    register,
    logout,
    isAuthenticated: !!token && !!user,
  };

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};

export const useAuth = (): AuthContextType => {
  const context = useContext(AuthContext);
  if (context === undefined) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
};
