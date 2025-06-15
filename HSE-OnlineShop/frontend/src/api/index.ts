// src/api/index.ts
import axios from "axios";
import { Order, Payment } from "../types";

// Единый axios-инстанс для API Gateway
const api = axios.create({
    baseURL: "http://localhost:8080/api",
});

// ----- Orders API -----

// Создать заказ
export const createOrder = async (
    order: Omit<Order, "id" | "status">
): Promise<Order> => {
    const response = await api.post<Order>("/orders", order);
    return response.data;
};

// Получить все заказы
export const getOrders = async (): Promise<Order[]> => {
    const response = await api.get<Order[]>("/orders");
    return response.data;
};

// Получить заказ по id (если потребуется)
export const getOrderById = async (orderId: number): Promise<Order> => {
    const response = await api.get<Order>(`/orders/${orderId}`);
    return response.data;
};

// ----- Payments API -----

// Получить все платежи
export const fetchPayments = async (): Promise<Payment[]> => {
    const response = await api.get<Payment[]>("/payments");
    return response.data;
};

// Получить платежи по id заказа (если нужно)
export const fetchPaymentsByOrder = async (orderId: number): Promise<Payment[]> => {
    const response = await api.get<Payment[]>(`/payments/order/${orderId}`);
    return response.data;
};

// Создать платеж (если нужно)
export const createPayment = async (
    payment: Omit<Payment, "id" | "status">
): Promise<Payment> => {
    const response = await api.post<Payment>("/payments", payment);
    return response.data;
};