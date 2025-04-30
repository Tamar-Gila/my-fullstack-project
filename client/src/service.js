import axios from 'axios';

//לשלוח לבסיס
// axios.defaults.baseURL = "http://localhost:5299/items"

axios.defaults.baseURL = process.env.REACT_AP_API;


//מטפל בשגיאות
axios.interceptors.response.use(
  (response) => {
    // במקרה של Response תקין (כלומר סטטוס 2xx), נחזיר את ה-response כמו שהוא
    return response;
  },
  (error) => {
    // במקרה של שגיאה ב-Response (סטטוס שגיאה), תתפוס את השגיאה ותרשום אותה ללוג
    console.error('API Error:', error.response ? error.response.data : error.message);

    // אם צריך, אפשר גם לשלוח את השגיאה הלאה למי שקורא לפונקציה
    return Promise.reject(error);  // חשוב להחזיר את השגיאה כדי שהקוד שקורא לפונקציה יוכל לטפל בה
  }
);

export default {
  getTasks: async () => {
    try {
      const result = await axios.get("/")
      return result.data;
    }
    catch (error) {
      console.error(error);
      throw error;
    }
  },

  addTask: async (name) => {
    try {
      console.log('addTask', name)
      const result = await axios.post("/", { name: name, isComplete: false }); // שולח בקשה להוספת משימה
      return result.data;
    }
    catch (error) {
      console.error(error);
      throw error;
    }
  },
//פונקציה שעוורכת אם הושלמה המשימה
  setCompleted: async (id, isComplete) => {
    try {
      console.log('setCompleted', { id, isComplete })
      const result = await axios.put(`/${id}`, { isComplete: isComplete });
      return result.data;
    }
    catch (error) {
      console.error(error);
      throw error;
    }
  },

  deleteTask: async (event) => {
    try {
      console.log('deleteTask')
      const result = await axios.delete(`/${event}`);
      return result.data;
    }
    catch (error) {
      console.error(error);
      throw error;
    }
  }
};