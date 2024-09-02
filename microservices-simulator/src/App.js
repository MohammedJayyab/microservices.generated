// src/App.js
import React, { useState } from "react";
import OrderComponent from "./OrderComponent/OrderComponent";
import PaymentComponent from "./PaymentComponent/PaymentComponent";
import NotificationComponent from "./NotificationComponent/NotificationComponent";

function App() {
  const [orderId, setOrderId] = useState(null);
  const [notifications, setNotifications] = useState([]);

  const handleOrderPlaced = (order) => {
    setOrderId(order.orderId);
    setNotifications((prev) => [
      ...prev,
      `Order ${order.orderId} placed successfully!`,
    ]);
  };

  return (
    <div className="App">
      <h1>Microservices Simulator</h1>
      <OrderComponent onOrderPlaced={handleOrderPlaced} />
      {orderId && <PaymentComponent orderId={orderId} />}
      <NotificationComponent notifications={notifications} />
    </div>
  );
}

export default App;
