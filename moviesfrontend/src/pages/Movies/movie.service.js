import axios from 'axios';

const APIURL = process.env.REACT_APP_MOVIES_API_URL + "/movies"

export const fetchMovies = () => {
    return axios(`${APIURL}/`)
};

// export function fetchLocalMapBox(local: string){
//     return axios(`https://api.mapbox.com/geocoding/v5/mapbox.places/${local}.json?access_token=${mapboxToken}`)
// }
//
// export function saveOrder(payload : OrderPayload){
//     return axios.post(`${APIURL}/orders`,payload);
// }