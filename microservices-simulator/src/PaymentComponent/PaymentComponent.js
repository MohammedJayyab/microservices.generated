// src/PaymentComponent.js
import React from "react";
import axios from "axios";

const PaymentComponent = ({ orderId }) => {
  const processPayment = async () => {
    try {
      const response = await axios.post(`http://localhost:6000/api/payments`, {
        orderId,
      });
      console.log("Payment processed successfully:", response.data);
    } catch (error) {
      console.error("Error processing payment:", error);
    }
  };

  return (
    <div>
      <h2>Process Payment</h2>
      <button onClick={processPayment}>Pay for Order {orderId}</button>
    </div>
  );
};

export default PaymentComponent;
