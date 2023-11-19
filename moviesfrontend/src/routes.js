import {BrowserRouter, Route, Switch} from 'react-router-dom';
import Movies from "./pages/home";
import Header from "./shared/components/Header/header";
import Movie from "./pages/movies/edit-movie";

const Routes = () =>{
    return(
        <BrowserRouter forceRefresh>
            <Header/>
            <Switch>
                <Route path="/" exact>
                    <Movies/>
                </Route>
                <Route path="/movies/:id" exact>
                    <Movie/>
                </Route>
            </Switch>
        </BrowserRouter>
    )
}

export default Routes;