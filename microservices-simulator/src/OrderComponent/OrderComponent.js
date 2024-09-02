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
        placeholder="Order ID"
        value={orderDetails.orderId}
        onChange={handleChange}
      />
      <input
        type="text"
        name="productName"
        placeholder="Product Name"
        value={orderDetails.productName}
        onChange={handleChange}
      />
      <input
        type="number"
        name="quantity"
        placeholder="Quantity"
        value={orderDetails.quantity}
        onChange={handleChange}
      />
      <input
        type="number"
        name="price"
        placeholder="Price per unit"
        value={orderDetails.price}
        onChange={handleChange}
      />
      <button onClick={placeOrder}>Place Order</button>
    </div>
  );
};

export default OrderComponent;
