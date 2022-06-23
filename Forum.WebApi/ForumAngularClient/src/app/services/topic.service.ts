import {HttpClient} from "@angular/common/http";
import {TopicModel} from "../models/topic/topic.model";
import {CreateTopicModel} from "../models/topic/create-topic.model";
import {UpdateTopicModel} from "../models/topic/update-topic.model";
import {Injectable} from "@angular/core";

@Injectable({
  providedIn: 'root'
})
export class TopicService {
  private baseUrl = 'https://localhost:5001/api/topics';

  constructor(private http: HttpClient) {
  }

  getAll() {
    return this.http.get<TopicModel[]>(this.baseUrl);
  }

  getById(id: number) {
    return this.http.get<TopicModel>(`${this.baseUrl}/${id}`);
  }

  add(model: CreateTopicModel) {
    return this.http.post<TopicModel>(this.baseUrl, model);
  }

  update(model: UpdateTopicModel) {
    return this.http.put(this.baseUrl, model);
  }

  delete(id: number) {
    return this.http.delete(`${this.baseUrl}/${id}`)
  }

  increaseRating(id: number) {
    return this.http.put(`${this.baseUrl}/${id}/increase-rating`, null)
  }

  decreaseRating(id: number) {
    return this.http.put(`${this.baseUrl}/${id}/decrease-rating`, null)
  }

  pin(id: number) {
    return this.http.put(`${this.baseUrl}/${id}/pin`, null)
  }

  unpin(id: number) {
    return this.http.put(`${this.baseUrl}/${id}/unpin`, null)
  }

  blockComments(id: any) {
    return this.http.put(`${this.baseUrl}/${id}/block-comments`, null)
  }
}
