import React from 'react';
import './App.css';
import { BrowserRouter as Router, Routes, Route, Navigate } from "react-router-dom";
import OrdersPage from "./pages/OrdersPage/OrdersPage";
import CreateOrderPage from "./pages/CreateOrderPage/CreateOrderPage";
import PaymentsPage from "./pages/PaymentsPage/PaymentsPage";
import { Layout, Menu } from "antd";

const { Header, Content } = Layout;

const App: React.FC = () => (
  <Router>
    <Layout>
      <Header>
        <Menu theme="dark" mode="horizontal" selectable={false}>
          <Menu.Item key="orders"><a href="/orders">Заказы</a></Menu.Item>
          <Menu.Item key="create-order"><a href="/create-order">Создать заказ</a></Menu.Item>
          <Menu.Item key="payments"><a href="/payments">Платежи</a></Menu.Item>
        </Menu>
      </Header>
      <Content style={{padding: '24px'}}>
        <Routes>
          <Route path="/" element={<Navigate to="/orders" replace />} />
          <Route path="/orders" element={<OrdersPage />} />
          <Route path="/create-order" element={<CreateOrderPage />} />
          <Route path="/payments" element={<PaymentsPage />} />
        </Routes>
      </Content>
    </Layout>
  </Router>
)

export default App;
