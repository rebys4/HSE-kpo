import React, {useEffect, useState} from 'react';
import {fetchPayments} from "../../api";
import {Payment} from "../../types";
import PaymentsTable from "../../components/PaymentsTable";

const PaymentsPage: React.FC = () => {
    const [payments, setPayments] = useState<Payment[]>([]);
    const [loading, setLoading] = useState<boolean>(true);

    useEffect(() => {
        fetchPayments()
            .then((data) => setPayments(data))
            .finally(() => setLoading(false));
    })

    return (
        <div style={{ padding: '24px' }}>
            <h1>Платежи</h1>
            <PaymentsTable payments={payments} loading={loading} />
        </div>
    );
};

export default PaymentsPage;