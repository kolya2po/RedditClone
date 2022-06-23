import {Pipe, PipeTransform} from "@angular/core";
import {UserModel} from "../../models/user/user.model";

@Pipe({
  name : 'usersFilter'
})
export class UsersFilterPipe implements PipeTransform{
  transform(users: UserModel[], search: string = ''): UserModel[] {
    if (!search.trim()) {
      return users;
    }

    return users.filter(u => {
      return u?.userName?.toLowerCase().indexOf(search.toLowerCase()) !== -1;
    })
  }

}
