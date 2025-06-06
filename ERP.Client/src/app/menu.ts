export class MenuModel{
    name: string="";
    icon: string="";
    url: string="";
    isTitle: boolean = false;
    subMenus: MenuModel[]=[];
}

export const Menus : MenuModel[] = [
    {
        name: "Ana Sayfa",
        icon: "far fa-solid fa-home",
        url: "/",
        isTitle: false,
        subMenus: []
    },
    {
        name: "Ana Group",
        icon: "fa-solid fa-trowel-bricks",
        url: "",
        isTitle: false,
        subMenus: [
            {
                name: "Müşteriler",
                icon: "far fa-solid fa-users",
                url: "/customers",
                isTitle: false,
                subMenus: []
            },
            {
                name: "Depo",
                icon: "far fa-solid fa-warehouse",
                url: "/depots",
                isTitle: false,
                subMenus: []
            },
            {
                name: "Ürünler",
                icon: "far fa-solid fa-boxes-stacked",
                url: "/products",
                isTitle: false,
                subMenus: []
            },
            {
                name: "Reçeteler",
                icon: "far fa-solid fa-boxes-packing",
                url: "/recipes",
                isTitle: false,
                subMenus: []
            }
        ]
    },
    {
        name: "Siparişler",
        icon: "far fa-solid fa-clipboard-list",
        url: "/orders",
        isTitle: false,
        subMenus: []
    },
    {
        name: "Faturalar",
        icon: "far fa-solid fa-file-invoice",
        url: "/",
        isTitle: false,
        subMenus: [
            {
                name: "Alış Faturaları",
                icon: "far fa-solid fa-clipboard-list",
                url: "/invoices/purchase",
                isTitle: false,
                subMenus: []
            },
            {
                name: "Satış Faturaları",
                icon: "far fa-solid fa-clipboard-list",
                url: "/invoices/selling",
                isTitle: false,
                subMenus: []
            }
        ]
    },
    {
        name: "Üretim",
        icon: "far fa-solid fa-screwdriver-wrench",
        url: "/productions",
        isTitle: false,
        subMenus: []
    }
]