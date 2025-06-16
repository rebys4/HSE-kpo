import React from 'react';
import {Payment, PaymentsTableProps} from "../types";
import {Table} from "antd";
import {ColumnsType} from "antd/es/table";

const columns: ColumnsType<Payment> = [
    {
        title: "ID",
        dataIndex: "id",
        key: "id",
    },
    {
        title: "Сумма",
        dataIndex: "amount",
        key: "amount",
        render: (value: number) => `${value} ₽`,
    },
    {
        title: "Статус",
        dataIndex: "status",
        key: "status",
    },
    {
        title: "Дата",
        dataIndex: "createdAt",
        key: "createdAt",
        render: (date: string) => new Date(date).toLocaleString(),
    },
];

const PaymentsTable: React.FC<PaymentsTableProps> = ({ payments, loading }) => (
    <Table
        dataSource={payments}
        columns={columns}
        loading={loading}
        rowKey="id"
    />
);

export default PaymentsTable;