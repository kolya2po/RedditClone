import {HttpClient, HttpErrorResponse} from "@angular/common/http";
import {RegistrationModel} from "../models/user/registration.model";
import {LoginModel} from "../models/user/login.model";
import {UpdateUserModel} from "../models/user/update-user.model";
import {UserModel} from "../models/user/user.model";
import {Injectable} from "@angular/core";
import {Router} from "@angular/router";
import {RouterExtService} from "../routing/routerExt.service";
import {CookieService} from 'ngx-cookie-service';
import {MatSnackBar} from "@angular/material/snack-bar";
import {catchError, tap} from "rxjs";

class UserDto {
  constructor(
    public id: number,
    public token: string
  ) {
  }
}

@Injectable({
  providedIn: 'root'
})
export class UserService {
  user = new UserModel();

  private baseUrl = 'https://localhost:5001/api/users';

  constructor(private http: HttpClient, private router: Router,
              private routeExt: RouterExtService,
              private cookieService: CookieService,
              private _snackBar: MatSnackBar) {

    if (cookieService.get('auth_token')) {
      let id = +cookieService.get('id');
      this.getById(id)
        .subscribe(u => this.user = u);
    }
  }

  getById(id: any) {
    return this.http.get<UserModel>(this.baseUrl + '/' + id);
  }

  registration(model: RegistrationModel) {
    return this.http.post<UserDto>(this.baseUrl + '/registration', model)
      .pipe(tap((dto: any) => {
      }), catchError((err) => {
        let resp = new HttpErrorResponse(err);
        if (resp.error.errors) {
          this.openSnackBar('Invalid birthdate');
          throw err;
        }
        this.openSnackBar(resp.error);
        throw err;
      })).subscribe(dto => {
        this.handleReturnedUser(dto);
      });
  }

  login(model: LoginModel) {
    return this.http.post<UserDto>(this.baseUrl + '/login', model)
      .pipe(tap((dto: any) => {
      }), catchError((err) => {
        let resp = new HttpErrorResponse(err);
        this.openSnackBar(resp.error);
        throw err;
      }))
      .subscribe(dto => {
         this.handleReturnedUser(dto);
      });
  }

  handleReturnedUser(dto: UserDto) {
    if (dto === null) {
      return;
    }
    localStorage.setItem('auth_token', dto.token);
    this.saveToCookies(dto);
    this.getById(dto.id)
      .subscribe(u => this.user = u);
    let prev = this.routeExt.getPreviousUrl();
    this.router.navigate([prev]);
  }

  logout() {
    return this.http.get(this.baseUrl)
      .subscribe(() => {
        this.user = new UserModel();
        this.cookieService.delete('auth_token');
        this.cookieService.delete('id');
        if (this.cookieService.get('auth_token')) {
          this.cookieService.deleteAll();
        }
        this.router.navigate(['/'])
      });
  }

  update(model: UpdateUserModel) {
    return this.http.put(this.baseUrl, model);
  }

  delay(ms: number) {
    return new Promise(resolve => setTimeout(resolve, ms));
  }

  saveToCookies(dto: UserDto) {
    this.cookieService.set('auth_token', dto.token,
      new Date(new Date().setHours(new Date().getHours() + 3)));
    this.cookieService.set('id', dto.id.toString());
  }

  openSnackBar(message: any) {
    this._snackBar.open(message, '', {
      duration: 2000
    });
  }
}
