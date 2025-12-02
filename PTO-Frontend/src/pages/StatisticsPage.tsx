import {
    Box,
    Center,
    Container, Divider, Group,
    LoadingOverlay,
    MultiSelect,
    Paper,
    Text,
    Title
} from "@mantine/core";
import {useEffect, useState} from "react";
import '@mantine/charts/styles.css';
import dayjs from 'dayjs';
import { notifications } from "@mantine/notifications";
import api from "../api/api.ts";
import {BarChart, LineChart, PieChart} from '@mantine/charts';
import { DateInput } from '@mantine/dates';
import type StatDetailGetDto from "../Interfaces/statDetailGetDto.ts";
import type thirdChart from "../Interfaces/thirdChart.ts";
import type secondChart from "../Interfaces/secondChart.ts";


function StatisticsPage () {
    const [isLoading,setIsLoading] = useState(false)

    const [firstChartData, setfirstChartData] = useState<StatDetailGetDto[] | []> ([])
    const [secondChartData, setSecondChartData] = useState<secondChart[] | []> ([])
    const [thirdChartData, setThirdChartData] = useState<thirdChart[] | []> ([])
    const [dropdownValue, setDropdownValue] = useState<string[]|undefined> (undefined)
    const [departmments, setDepartmments] = useState<string[]> ([])
    const [value, setValue] = useState<string | null>( dayjs().format('YYYY-MM-DD'));


    const fetchData = async (date:string) => {
        setIsLoading(true);
        try{
            const departmentdata = await api.Department.getDepartments();
            const l_departments = departmentdata.data?.data ?? []
            setDepartmments(l_departments);

            const statistics_data = await api.Stats.postStatsProDepartment({date:(date)});
            const statData_data = statistics_data.data?.data ?? []


            const weekly_data = await api.Stats.postStatsForWeek({date:(date)});
            const weeklyData_data = weekly_data.data?.data ?? []


            if(statData_data.length === 0 || weeklyData_data.length === 0){
                console.error("empty response from server")


            }

            const listForsDetails: StatDetailGetDto[] = [];
            const listForThird: thirdChart[] = [];
            for(let i = 0; i < statData_data.length; i++){
                listForsDetails.push(statData_data[i].details)
                listForThird.push({name: statData_data[i].name, value:statData_data[i].value, color: statData_data[i].color})
            }

            setSecondChartData(weeklyData_data);
            setfirstChartData(listForsDetails);
            setThirdChartData(listForThird);



        }
        catch(error){
            console.log(error);
            notifications.show({
                title:"Hiba",
                message:"Hiba történt a lekérés során!",
                color:"red"
            })
        }
        finally {
            setIsLoading(false)
        }
    }


    const dataforchart =  () => {
        if (!serachParameters || serachParameters.length === 0) {

            return firstChartData;
        }
       return firstChartData.filter(c => serachParameters?.includes(c.department));
    }

    useEffect(() => {
        if (value) {
            const tempval = value.toString()
            fetchData(tempval);
        }
    }, [value]);



    useEffect(() => {
        fetchData((dayjs(value).format('YYYY-MM-DD') ?? dayjs().format('YYYY-MM-DD')));

    }, []);


    const [serachParameters, setSearchParameters] = useState<string[] | undefined> (undefined)


    {
        useEffect(() => {
            setSearchParameters(dropdownValue)
        }, [dropdownValue]);
    }


    return(
        <Container>
            <Paper p="xl" radius="md" withBorder pos = "relative">
                <LoadingOverlay visible={isLoading} overlayProps={{ blur: 2 }} />
                <Title style={{fontSize:20, textAlign:"center"}}>Kérem válasszon mely részlegek kérelmeit szeretné látni!</Title>
                <Group justify={"space-between"}>
                    <MultiSelect
                        label="Részlegek választása"
                        placeholder={"pl.: IT"}
                        data = {departmments}
                        value={dropdownValue}
                        onChange={setDropdownValue}
                        searchable
                        mt={10}
                    />
                    <DateInput
                        label="Dátum"
                        placeholder="Válassz dátumot"
                        value={value}
                        onChange={setValue}
                    />
                </Group>
                <Center>
                    <Box style={{ overflow: "auto"}} mt={20} w="100%">
                        <Divider my="lg"/>
                        <Center>
                            <Text fw={"bold"} mb={30}>Szabadságok lebontása részlegenként</Text>
                        </Center>
                        <BarChart
                            h={300}
                            data={dataforchart()}
                            dataKey="department"
                            valueFormatter={(value) => new Intl.NumberFormat('en-US').format(value)}
                            withBarValueLabel
                            series={[
                                { name: 'betegSzab', label:'Beteg szabadság', color: 'violet.6' },
                                { name: 'pto',  label:'Fizetett szabadság', color: 'blue.6' },
                                { name: 'kikuldetes', label:'Kiküldetés', color: 'teal.6' },
                            ]}
                            withYAxis={false}
                        />
                    </Box>
                </Center>
                <Divider my="lg"/>
                <Center>
                    <Text fw={"bold"} mb={30}>Céges szintű szabadság statisztika</Text>
                </Center>
                    <Box>
                        <LineChart
                            h={300}
                            data={secondChartData}
                            dataKey="date"
                            withLegend
                            legendProps={{ verticalAlign: 'bottom', height: 50 }}
                            series={[
                                { name: 'betegSzab', label:'Beteg szabadság', color: 'violet.6' },
                                { name: 'pto',  label:'Fizetett szabadság', color: 'blue.6' },
                                { name: 'kikuldetes', label:'Kiküldetés', color: 'teal.6' },
                            ]}
                        />
                    </Box>
                <Divider my="lg"/>
                <Center>
                    <Text fw={"bold"} mb={10}>Részlegek közötti szabadságmegoszlás</Text>
                </Center>
                <Center>
                    <Box>
                        <PieChart
                            withLabelsLine
                            labelsPosition="outside"
                            labelsType="value"
                            withLabels
                            withTooltip={true}
                            tooltipDataSource="segment"
                            size={200}
                            data={thirdChartData} />
                        <Group justify="center" mt="md">
                            {thirdChartData.map((item) => (
                                <Group key={item.name} gap="xs" align="center">
                                    <Box h={15} w={15} bg={item.color} />
                                    <Text fw="bold">{item.name}</Text>
                                </Group>
                            ))}
                        </Group>
                    </Box>
                </Center>
            </Paper>
        </Container>
    );
}

export default StatisticsPage;