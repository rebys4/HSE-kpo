import React, { useState, useEffect } from 'react';
import "./OrdersPage.scss"
import { useNavigate } from 'react-router-dom';
import {Order} from "../../types";
import {Button} from "antd";
import OrderTable from "../../components/OrderTable";
import {getOrders} from "../../api";

const OrdersPage: React.FC = () => {
    const [orders, setOrders] = useState<Order[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const navigate = useNavigate();

    useEffect(() => {
        setLoading(true);
        // getOrders()
        //     .then(setOrders)
        //     .finally(() => setLoading(false));
    }, []);

    return (
        <div className="orders-page">
            <div className="orders-page__header">
                <h1>Заказы</h1>
                <Button type="primary" onClick={() => navigate('/create-order')}>
                    Создать заказ
                </Button>
            </div>
            <OrderTable orders={orders} loading={loading} />
        </div>
    );
};

export default OrdersPage;