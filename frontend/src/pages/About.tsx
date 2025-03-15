import React, { useEffect, useState } from "react";
import axios from "axios";

const About: React.FC = () => {
  const [aboutText, setAboutText] = useState("");
  const [isEditing, setIsEditing] = useState(false);
  const [userRole, setUserRole] = useState("");

  useEffect(() => {
    fetchAboutText();

    // Check Role
    const token = localStorage.getItem("token");
    if (token) {
      const decodedToken = JSON.parse(atob(token.split(".")[1]));
      setUserRole(decodedToken.role || "User");
    }
  }, []);

  const fetchAboutText = async () => {
    try {
      const response = await axios.get("https://localhost:7167/api/about");
      setAboutText(response.data.text);
    } catch (error) {
      console.error("Info loading error:", error);
    }
  };

  const handleSave = async () => {
    try {
      const token = localStorage.getItem("token");
      await axios.post(
        "https://localhost:7167/api/about",
        { text: aboutText },
        { headers: { Authorization: `Bearer ${token}` } }
      );
      setIsEditing(false);
    } catch (error) {
      console.error("Saving Error:", error);
    }
  };

  return (
    <div>
      <h2>About URL Shortener Algorithm</h2>
      {isEditing ? (
        <textarea value={aboutText} onChange={(e) => setAboutText(e.target.value)} rows={5} cols={50} />
      ) : (
        <p>{aboutText}</p>
      )}
      {userRole === "Admin" && (
        <div>
          {isEditing ? (
            <button onClick={handleSave}>Save</button>
          ) : (
            <button onClick={() => setIsEditing(true)}>Edit</button>
          )}
        </div>
      )}
    </div>
  );
};

export default About;

export {};