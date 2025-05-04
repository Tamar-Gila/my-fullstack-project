const express = require('express');
const renderApi = require('api')('@render-api/v1.0#98nonzmma6v5ra7');

const app = express();
const PORT = process.env.PORT || 3000;

renderApi.auth('rnd_jtnUDY8x2ORFgjivVDTC5WpVT473');

app.get('/', async (req, res) => {
  try {
    const { data } = await renderApi.listServices({
      includePreviews: 'true',
      limit: '20'
    });
    res.json(data);
  } catch (err) {
    console.error('שגיאה בשליפת רשימת אפליקציות:', err);
    res.status(500).json({ error: 'שגיאה בקבלת האפליקציות מ-Render' });
  }
});

app.listen(PORT, () => {
    console.log( 'wow' );
    
});