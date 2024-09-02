import React, { useState } from "react";
import axios from "axios";

const PaymentComponent = ({ orderId }) => {
  const [paymentDetails, setPaymentDetails] = useState({
    amount: 0,
    paymentMethod: "CreditCard", // Default payment method
  });

  const handleChange = (e) => {
    setPaymentDetails({ ...paymentDetails, [e.target.name]: e.target.value });
  };

  const processPayment = async () => {
    try {
      const paymentRequest = {
        orderId: orderId,
        amount: paymentDetails.amount,
        paymentMethod: paymentDetails.paymentMethod,
      };

      const response = await axios.post(
        "http://localhost:6090/api/payment",
        paymentRequest
      );
      console.log("Payment processed successfully:", response.data);
    } catch (error) {
      console.error("Error processing payment:", error);
    }
  };

  return (
    <div>
      <h2>Process Payment for Order {orderId}</h2>
      <input
        type="number"
        name="amount"
        placeholder="Amount"
        value={paymentDetails.amount}
        onChange={handleChange}
      />
      <select
        name="paymentMethod"
        value={paymentDetails.paymentMethod}
        onChange={handleChange}
      >
        <option value="CreditCard">Credit Card</option>
        <option value="PayPal">PayPal</option>
        <option value="BankTransfer">Bank Transfer</option>
      </select>
      <button onClick={processPayment}>Pay</button>
    </div>
  );
};

export default PaymentComponent;
