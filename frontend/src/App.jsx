import { useEffect, useState } from 'react';
import axios from 'axios';
import ImageGallery from 'react-image-gallery';
import 'react-image-gallery/styles/css/image-gallery.css';

function App() {
    const [photos, setPhotos] = useState([]);
    const [selectedPhoto, setSelectedPhoto] = useState(null);

    useEffect(() => {
        axios.get('http://localhost:5000/api/photos')
            .then(response => setPhotos(response.data))
            .catch(error => console.error(error));
    }, []);

    const images = photos.map(photo => ({
        original: `http://localhost:5000/${photo.url}`,
        thumbnail: `http://localhost:5000/${photo.url}`,
        description: `EXIF: ${photo.exifData}`
    }));

    return (
        <div>
            <h1>Photo Gallery</h1>
            <ImageGallery items={images} />
            {selectedPhoto && (
                <div>
                    <h2>{selectedPhoto.title}</h2>
                    <img src={`http://localhost:5000/${selectedPhoto.url}`} alt={selectedPhoto.title} />
                    <p>EXIF Data: {selectedPhoto.exifData}</p>
                </div>
            )}
        </div>
    );
}
export default App;