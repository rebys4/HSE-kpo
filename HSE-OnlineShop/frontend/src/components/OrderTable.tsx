import React from 'react';
import { Order } from "../types";
import {OrderTableProps} from "../types";
import {Table} from "antd";
import {ColumnsType} from "antd/es/table";

const columns: ColumnsType<Order> = [
    {
        title: "ID",
        dataIndex: "id",
        key: "id",
    },
    {
        title: "Покупатель",
        dataIndex: "customerName",
        key: "customerName",
    },
    {
        title: "Товар",
        dataIndex: "product",
        key: "product",
    },
    {
        title: "Кол-во",
        dataIndex: "quantity",
        key: "quantity",
    },
    {
        title: "Статус",
        dataIndex: "status",
        key: "status",
    },
    {
        title: "Создан",
        dataIndex: "createdAt",
        key: "createdAt",
        render: (date: string) => new Date(date).toLocaleString(),
    },
];

const OrderTable: React.FC<OrderTableProps> = ({ orders, loading}) => {
    return (
        <Table
            dataSource={orders}
            loading={loading}
            rowKey="id"
            columns={columns}
            pagination={false}
        />
    );
};

export default OrderTable;