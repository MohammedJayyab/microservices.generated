// src/OrderComponent.js
import React, { useState } from "react";
import axios from "axios";

const OrderComponent = ({ onOrderPlaced }) => {
  const [orderDetails, setOrderDetails] = useState({
    orderId: "",
    productName: "",
    quantity: 1,
    price: 0,
  });

  const handleChange = (e) => {
    setOrderDetails({ ...orderDetails, [e.target.name]: e.target.value });
  };

  const placeOrder = async () => {
    try {
      const response = await axios.post(
        "http://localhost:5090/api/orders",
        orderDetails
      );
      console.log("Order placed successfully:", response.data);
      onOrderPlaced(response.data); // Notify parent component
    } catch (error) {
      console.error("Error placing order:", error);
    }
  };

  return (
    <div>
      <h2>Place Order</h2>
      <input
        type="text"
        name="orderId"
        placeholder="order Id"
        onChange={handleChange}
      />
      <input
        type="text"
        name="productName"
        placeholder="Product Name"
        onChange={handleChange}
      />
      <input
        type="number"
        name="quantity"
        placeholder="Quantity"
        onChange={handleChange}
      />
      <input
        type="number"
        name="price"
        placeholder="price per unit"
        onChange={handleChange}
      />
      <button onClick={placeOrder}>Place Order</button>
    </div>
  );
};

export default OrderComponent;
