import {HttpClient} from "@angular/common/http";
import {CreateCommunityModel} from "../models/community/create-community.model";
import {UpdateCommunityModel} from "../models/community/update-community.model";
import {CreateRuleModel} from "../models/rule/create-rule.model";
import {UpdateRuleModel} from "../models/rule/update-rule.model";
import {CommunityModel} from "../models/community/community.model";
import {UserModel} from "../models/user/user.model";
import {RuleModel} from "../models/rule/rule.model";
import {Injectable} from "@angular/core";

@Injectable({
  providedIn: 'root'
})
export class CommunityService {
  private baseUrl = 'https://localhost:5001/api/communities';
  constructor(private http: HttpClient) {
  }

  getAll() {
    return this.http.get<CommunityModel[]>(this.baseUrl);
  }

  getById(id: number) {
    return this.http.get<CommunityModel>(`${this.baseUrl}/${id}`);
  }

  getAllUsers(id: number) {
    return this.http.get<UserModel[]>(`${this.baseUrl}/${id}/users`);
  }

  add(model: CreateCommunityModel) {
    return this.http.post<CommunityModel>(this.baseUrl, model);
  }

  update(model: UpdateCommunityModel) {
    return this.http.put(this.baseUrl, model);
  }

  delete(id: number) {
    return this.http.delete(`${this.baseUrl}/${id}`);
  }

  addModerator(userId: number, communityId: number) {
    return this.http.post(`${this.baseUrl}/add-moderator?userId=${userId}&communityId=${communityId}`, null);
  }

  removeModerator(userId: number, communityId: number) {
    return this.http.delete(`${this.baseUrl}/remove-moderator?userId=${userId}&communityId=${communityId}`);
  }

  join(userId: any, communityId: number) {
    return this.http.post(`${this.baseUrl}/join?userId=${userId}&communityId=${communityId}`, null);
  }

  leave(userId: number, communityId: number) {
    return this.http.delete(`${this.baseUrl}/leave?userId=${userId}&communityId=${communityId}`);
  }

  addRule(model: CreateRuleModel) {
    return this.http.post<RuleModel>(`${this.baseUrl}/add-rule`, model);
  }

  updateRule(model: UpdateRuleModel) {
    return this.http.put(`${this.baseUrl}/update-rule`, model);
  }

  deleteRule(id: number) {
    return this.http.delete(`${this.baseUrl}/delete-rule/${id}`);
  }
}
