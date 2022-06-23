import { NgModule } from '@angular/core';
import {RouterModule, Routes} from "@angular/router";
import {MainPageComponent} from "../components/main-page/main-page.component";
import {LoginComponent} from "../components/auth/login/login.component";
import {RegistrationComponent} from "../components/auth/registration/registration.component";
import {CommunityPageComponent} from "../components/community-page/community-page.component";
import {UserPageComponent} from "../components/user-page/user-page.component";
import {TopicPageComponent} from "../components/topic-page/topic-page.component";
import {CreateTopicPageComponent} from "../components/create-topic-page/create-topic-page.component";
import {CommunityUsersPageComponent} from "../components/community-users-page/community-users-page.component";
import {CommunitiesPageComponent} from "../components/communities-page/communities-page.component";
import {CreateCommunityPageComponent} from "../components/create-community-page/create-community-page.component";
import {ControlCommunityPageComponent} from "../components/control-community-page/control-community-page.component";
import {UpdateTopicPageComponent} from "../components/update-topic-page/update-topic-page.component";

const routes: Routes = [
  { path: '', component: MainPageComponent },
  { path: 'login', component: LoginComponent },
  { path: 'registration', component: RegistrationComponent },
  { path: 'user/:id', component: UserPageComponent },

  { path: 'community/:id', component: CommunityPageComponent },
  { path: 'community/users/:name/:id', component: CommunityUsersPageComponent },
  { path: 'communities', component: CommunitiesPageComponent },
  { path: 'create-community', component: CreateCommunityPageComponent },
  { path: 'community/:id/control', component: ControlCommunityPageComponent },

  { path: 'topic/:id', component: TopicPageComponent },
  { path: 'create-topic/:id', component: CreateTopicPageComponent },
  { path: 'create-topic', component: CreateTopicPageComponent },
  { path: 'update-topic/:id', component: UpdateTopicPageComponent },
]

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forRoot(routes)
  ],
  exports : [
    RouterModule
  ]
})
export class RoutingModule { }
