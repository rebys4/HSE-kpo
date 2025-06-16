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
        title: "userID",
        dataIndex: "userId",
        key: "userId",
    },
    {
        title: "Описание",
        dataIndex: "description",
        key: "description",
    },
    {
        title: "Кол-во",
        dataIndex: "amount",
        key: "amount",
    },
    {
        title: "Статус",
        dataIndex: "status",
        key: "status",
    },
];

const OrderTable: React.FC<OrderTableProps> = ({ orders, loading}) => {
    console.log(orders);
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