import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import axios from "axios";

const Info: React.FC = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const [urlData, setUrlData] = useState<any>(null);
  const [isLoggedIn, setIsLoggedIn] = useState(false);

  useEffect(() => {
    const token = localStorage.getItem("token");
    if (!token) {
      navigate("/login"); // Return to login
      return;
    }
    setIsLoggedIn(true);

    const fetchUrlData = async () => {
      try {
        const response = await axios.get(`https://localhost:7167/api/urls/${id}`, {
          headers: { Authorization: `Bearer ${token}` }
        });
        setUrlData(response.data);
      } catch (error) {
        console.error("Data load error:", error);
      }
    };

    fetchUrlData();
  }, [id, navigate]);

  if (!isLoggedIn) {
    return null; // No render before redirection
  }

  if (!urlData) {
    return <p>Loading...</p>;
  }

  return (
    <div>
      <h2>Short URL Info</h2>
      <p><strong>Short URL:</strong> {urlData.shortUrl}</p>
      <p><strong>Created By:</strong> {urlData.userID || "---"}</p>
      <p><strong>Created At:</strong> {new Date(urlData.creationDate).toLocaleString()}</p>
    </div>
  );
};

export default Info;
export {};