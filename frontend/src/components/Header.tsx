import React, { useEffect, useState } from "react";
import { Link } from "react-router-dom";

const Header: React.FC = () => {
  const [isLoggedIn, setIsLoggedIn] = useState(false);

  useEffect(() => {
    const token = localStorage.getItem("token");
    setIsLoggedIn(!!token); // Check token
  }, []);

  const handleLogout = () => {
    localStorage.removeItem("token");
    setIsLoggedIn(false);
    window.location.href = "/"; // Reload page
  };

  return (
    <header style={styles.header}>
      <h1>URL Shortener</h1>
      <div>
        {isLoggedIn ? (
          <button onClick={handleLogout} style={styles.button}>Log Out</button>
        ) : (
          <>
            <Link to="/login" style={styles.button}>Log In</Link>
            <Link to="/register" style={styles.button}>Sign Up</Link>
            <Link to="/about" style={styles.button}>About</Link>
          </>
        )}
      </div>
    </header>
  );
};

const styles = {
  header: 
  { 
    display: "flex", 
    justifyContent: "space-between", 
    padding: "10px 20px", 
    background: "#282c34", 
    color: "white" 
},
  button: 
{ 
    marginLeft: "10px", 
    padding: "8px 15px", 
    background: "#4CAF50", 
    color: "white", 
    textDecoration: "none", 
    borderRadius: "5px", 
    cursor: "pointer" 
}
};

export default Header;