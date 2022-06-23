import {Pipe, PipeTransform} from "@angular/core";
import {CommunityModel} from "../../models/community/community.model";

@Pipe({
  name : 'communitiesFilter'
})
export class CommunitiesFilterPipe implements PipeTransform {
  transform(communities: CommunityModel[], search: string = ''): CommunityModel[] {
    if (!search.trim()) {
      return communities;
    }

    return communities.filter(c => {
      return c.title?.toLowerCase().indexOf(search.toLowerCase()) !== -1;
    })
  }
}
