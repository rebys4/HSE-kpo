export interface Order {
    id?: number;
    customerName: string;
    amount: number;
    status?: string;
}

export interface OrderTableProps {
    orders: Order[];
    loading: boolean;
}

export interface Payment {
    id: number;
    orderId: number;
    amount: number;
    status: string;
    createdAt: string;
}

export interface PaymentsTableProps {
    payments: Payment[];
    loading: boolean;
}