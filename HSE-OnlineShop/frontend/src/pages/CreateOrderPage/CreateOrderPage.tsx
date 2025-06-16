import * as React from 'react';
import "./CreateOrderPage.scss"
import {Button, Card, Form, Input, InputNumber, notification} from "antd";
import {Order} from "../../types";
import {createOrder} from "../../api";
import {useNavigate} from "react-router-dom";

const CreateOrderPage: React.FC = () => {

    const navigate = useNavigate();

    const [form] = Form.useForm();
    const onFinish = async (values: Omit<Order, "id" | "status" | "userId">): Promise<void> => {
        try {
            const userId = Math.floor(Math.random() * 1000) + 1;
            const orderData: Omit<Order, "id"> = {
                ...values,
                userId,
                status: "NEW"
            };
            await createOrder(orderData);
            notification.success({message: "Заказ успешно создан!"});
            form.resetFields();
            navigate("/orders");
        } catch {
            notification.error({message: "Ошибка при создании заказа. Пожалуйста, попробуйте еще раз."});
        }
    };

    return (
        <div className="create-order-wrapper">
            <Card title="Создание заказа" className="create-order-card">
                <Form form={form} name="create-order" onFinish={onFinish} layout="vertical" >
                    <Form.Item
                        label="Сумма"
                        name="amount"
                        rules={[{ required: true, message: 'Пожалуйста, введите сумму!' }]}
                    >
                        <InputNumber min={0} style={{width: "100%"}}/>
                    </Form.Item>
                    <Form.Item
                        label="Описание"
                        name="description"
                        rules={[{ required: true, message: 'Пожалуйста, введите описание!' }]}
                    >
                        <Input />
                    </Form.Item>
                    <Form.Item>
                        <Button type="primary" htmlType="submit" block>
                            Создать заказ
                        </Button>
                    </Form.Item>
                </Form>
            </Card>
        </div>
    );
};

export default CreateOrderPage;