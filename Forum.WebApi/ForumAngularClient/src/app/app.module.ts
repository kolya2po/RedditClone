import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';

import {AppComponent} from './app.component';
import {LoginComponent} from './components/auth/login/login.component';
import {RegistrationComponent} from './components/auth/registration/registration.component';
import {FormsModule} from "@angular/forms";
import {MainPageComponent} from './components/main-page/main-page.component';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {CommunityPageComponent} from './components/community-page/community-page.component';
import {UserPageComponent} from './components/user-page/user-page.component';
import {HeaderComponent} from './components/header/header.component';
import {MatMenuModule} from "@angular/material/menu";
import {MatExpansionModule} from "@angular/material/expansion";
import {MatTabsModule} from "@angular/material/tabs";
import {TopicPageComponent} from './components/topic-page/topic-page.component';
import {CreateTopicPageComponent} from './components/create-topic-page/create-topic-page.component';
import {MatFormFieldModule} from "@angular/material/form-field";
import {MatSelectModule} from "@angular/material/select";
import {JwtModule} from '@auth0/angular-jwt';
import {RoutingModule} from "./routing/routing.module";
import { CommunityUsersPageComponent } from './components/community-users-page/community-users-page.component';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { CommunitiesPageComponent } from './components/communities-page/communities-page.component';
import {CommunitiesFilterPipe} from "./components/communities-page/communities-filter.pipe";
import {UsersFilterPipe} from "./components/community-users-page/users-filter.pipe";
import {HttpClientModule} from "@angular/common/http";
import { CreateCommunityPageComponent } from './components/create-community-page/create-community-page.component';
import { ControlCommunityPageComponent } from './components/control-community-page/control-community-page.component';
import { UpdateTopicPageComponent } from './components/update-topic-page/update-topic-page.component';
import { TopCommunitiesComponent } from './components/top-communities/top-communities.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegistrationComponent,
    MainPageComponent,
    CommunityPageComponent,
    UserPageComponent,
    HeaderComponent,
    TopicPageComponent,
    CreateTopicPageComponent,
    CommunityUsersPageComponent,
    CommunitiesPageComponent,
    CommunitiesFilterPipe,
    UsersFilterPipe,
    CreateCommunityPageComponent,
    ControlCommunityPageComponent,
    UpdateTopicPageComponent,
    TopCommunitiesComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    BrowserAnimationsModule,
    HttpClientModule,
    MatMenuModule,
    MatExpansionModule,
    MatTabsModule,
    MatFormFieldModule,
    MatSelectModule,
    RoutingModule,
    MatSnackBarModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: () => {
          return localStorage.getItem('auth_token');
        },
        allowedDomains: ['localhost:5001']
      }
    })
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {
}
