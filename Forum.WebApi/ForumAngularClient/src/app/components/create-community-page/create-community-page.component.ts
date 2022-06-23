import { Component } from '@angular/core';
import {CreateCommunityModel} from "../../models/community/create-community.model";
import {UserService} from "../../services/user.service";
import {CommunityService} from "../../services/community.service";
import {RouterExtService} from "../../routing/routerExt.service";
import {CookieService} from "ngx-cookie-service";

@Component({
  selector: 'app-create-community-page',
  templateUrl: './create-community-page.component.html',
  styleUrls: ['./create-community-page.component.css']
})
export class CreateCommunityPageComponent {
  createCommunityModel = new CreateCommunityModel();

  constructor(private us: UserService,
              private cs: CommunityService,
              private routeExt: RouterExtService,
              private cookieService: CookieService) { }

  createCommunity() {
    this.createCommunityModel.creatorId = this.us.user.id;

    this.cs.add(this.createCommunityModel)
      .subscribe(() => {
        this.us.getById(this.us.user.id)
          .subscribe(u => {
            this.us.user = u;
            // @ts-ignore
            localStorage.setItem('auth_token', this.us.user.token);
            // @ts-ignore
            this.cookieService.set('auth_token', this.us.user.token,
              new Date(new Date().setHours(new Date().getHours() + 3)));

            this.goToPreviousPage();
          });
      });
  }

  goToPreviousPage() {
    let prev = this.routeExt.getPreviousUrl();
    this.routeExt.router.navigate([prev]);
  }
}
