import {BrowserRouter, Route, Switch} from 'react-router-dom';
import Movies from "./pages/movies";
import Header from "./shared/components/Header/header";
import Movie from "./pages/movies/edit-movie";

const Routes = () =>{
    return(
        <BrowserRouter>
            <Header/>
            <Switch>
                <Route exact path='/' component={Movies}/>
                <Route exact path='/filme/:id' component={Movie}/>
            </Switch>
        </BrowserRouter>
    )
}

export default Routes;