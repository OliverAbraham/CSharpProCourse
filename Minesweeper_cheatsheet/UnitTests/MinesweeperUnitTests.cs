using System;
using System.IO;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MinesweeperLogic;

namespace UnitTests
{
    [TestClass]
    public class MinesweeperUnitTests
    {
        private MinesweeperEngine _Sut;
        private string _TempDirectory;
        private string _Input_Filename;

        [TestInitialize]
        public void Setup()
        {
            _Sut = new MinesweeperEngine();
            _TempDirectory = Environment.GetEnvironmentVariable("TEMP");
        }

        [TestMethod]
        public void Read_playground_from_file___test()
        {
            // ARRANGE
            WriteToDisk("*...\r\n....\r\n.*..\r\n....\r\n");

            // ACT
            _Sut.Read_playground_from_file(_Input_Filename);

            // ASSERT
            _Sut.Get_playground_cell(0,0).Should().BeTrue();
            _Sut.Get_playground_cell(1,0).Should().BeFalse();
            _Sut.Get_playground_cell(2,0).Should().BeFalse();
            _Sut.Get_playground_cell(3,0).Should().BeFalse();

            _Sut.Get_playground_cell(0,1).Should().BeFalse();
            _Sut.Get_playground_cell(1,1).Should().BeFalse();
            _Sut.Get_playground_cell(2,1).Should().BeFalse();
            _Sut.Get_playground_cell(3,1).Should().BeFalse();

            _Sut.Get_playground_cell(0,2).Should().BeFalse();
            _Sut.Get_playground_cell(1,2).Should().BeTrue();
            _Sut.Get_playground_cell(2,2).Should().BeFalse();
            _Sut.Get_playground_cell(3,2).Should().BeFalse();

            _Sut.Get_playground_cell(0,3).Should().BeFalse();
            _Sut.Get_playground_cell(1,3).Should().BeFalse();
            _Sut.Get_playground_cell(2,3).Should().BeFalse();
            _Sut.Get_playground_cell(3,3).Should().BeFalse();
        }

        [TestMethod]
        public void Write_cheatsheet_to_disk___test()
        {
            // ARRANGE
            _Sut.Create_cheatsheet_memory__for_unit_testing_only(4);
            _Sut.Set_cheatsheet_cell(0,0,2);
            _Sut.Set_cheatsheet_cell(1,1,3);
            _Sut.Set_cheatsheet_cell(2,2,4);
            _Sut.Set_cheatsheet_cell(3,3,5);

            // ACT
            string Output_Filename = _TempDirectory + Path.DirectorySeparatorChar + "MinesweeperUnitTests";
            File.Delete(Output_Filename);
            _Sut.Write_cheatsheet_to_disk(Output_Filename);

            // ASSERT
            string Contents = File.ReadAllText(Output_Filename);
            Contents.Should().Be("2000\r\n0300\r\n0040\r\n0005\r\n");
        }

        [TestMethod]
        public void Create_cheatsheet_test_1___no_mine()
        {
            // ARRANGE
            WriteToDisk("....\r\n....\r\n....\r\n....\r\n");
            _Sut.Read_playground_from_file(_Input_Filename);

            // ACT
            _Sut.Create_cheatsheet();

            // ASSERT
            _Sut.Get_cheatsheet_cell(0,0).Should().Be(0);
            _Sut.Get_cheatsheet_cell(1,0).Should().Be(0);
            _Sut.Get_cheatsheet_cell(2,0).Should().Be(0);
            _Sut.Get_cheatsheet_cell(3,0).Should().Be(0);
                                                   
            _Sut.Get_cheatsheet_cell(0,1).Should().Be(0);
            _Sut.Get_cheatsheet_cell(1,1).Should().Be(0);
            _Sut.Get_cheatsheet_cell(2,1).Should().Be(0);
            _Sut.Get_cheatsheet_cell(3,1).Should().Be(0);
                                                  
            _Sut.Get_cheatsheet_cell(0,2).Should().Be(0);
            _Sut.Get_cheatsheet_cell(1,2).Should().Be(0);
            _Sut.Get_cheatsheet_cell(2,2).Should().Be(0);
            _Sut.Get_cheatsheet_cell(3,2).Should().Be(0);
                                                   
            _Sut.Get_cheatsheet_cell(0,3).Should().Be(0);
            _Sut.Get_cheatsheet_cell(1,3).Should().Be(0);
            _Sut.Get_cheatsheet_cell(2,3).Should().Be(0);
            _Sut.Get_cheatsheet_cell(3,3).Should().Be(0);
        }

        [TestMethod]
        public void Create_cheatsheet_test_2___one_mine()
        {
            // ARRANGE
            WriteToDisk("....\r\n.*..\r\n....\r\n....\r\n");
            _Sut.Read_playground_from_file(_Input_Filename);

            // ACT
            _Sut.Create_cheatsheet();

            // ASSERT
            _Sut.Get_cheatsheet_cell(0,0).Should().Be(1);
            _Sut.Get_cheatsheet_cell(1,0).Should().Be(1);
            _Sut.Get_cheatsheet_cell(2,0).Should().Be(1);
            _Sut.Get_cheatsheet_cell(3,0).Should().Be(0);
                                                   
            _Sut.Get_cheatsheet_cell(0,1).Should().Be(1);
            _Sut.Get_cheatsheet_cell(1,1).Should().Be(1);
            _Sut.Get_cheatsheet_cell(2,1).Should().Be(1);
            _Sut.Get_cheatsheet_cell(3,1).Should().Be(0);
                                                  
            _Sut.Get_cheatsheet_cell(0,2).Should().Be(1);
            _Sut.Get_cheatsheet_cell(1,2).Should().Be(1);
            _Sut.Get_cheatsheet_cell(2,2).Should().Be(1);
            _Sut.Get_cheatsheet_cell(3,2).Should().Be(0);
                                                   
            _Sut.Get_cheatsheet_cell(0,3).Should().Be(0);
            _Sut.Get_cheatsheet_cell(1,3).Should().Be(0);
            _Sut.Get_cheatsheet_cell(2,3).Should().Be(0);
            _Sut.Get_cheatsheet_cell(3,3).Should().Be(0);
        }

        [TestMethod]
        public void Create_cheatsheet_test_3___two_mines()
        {
            // ARRANGE
            WriteToDisk("....\r\n.*.*\r\n....\r\n....\r\n");
            _Sut.Read_playground_from_file(_Input_Filename);

            // ACT
            _Sut.Create_cheatsheet();

            // ASSERT
            _Sut.Get_cheatsheet_cell(0,0).Should().Be(1);
            _Sut.Get_cheatsheet_cell(1,0).Should().Be(1);
            _Sut.Get_cheatsheet_cell(2,0).Should().Be(2);
            _Sut.Get_cheatsheet_cell(3,0).Should().Be(1);
                                                   
            _Sut.Get_cheatsheet_cell(0,1).Should().Be(1);
            _Sut.Get_cheatsheet_cell(1,1).Should().Be(1);
            _Sut.Get_cheatsheet_cell(2,1).Should().Be(2);
            _Sut.Get_cheatsheet_cell(3,1).Should().Be(1);
                                                  
            _Sut.Get_cheatsheet_cell(0,2).Should().Be(1);
            _Sut.Get_cheatsheet_cell(1,2).Should().Be(1);
            _Sut.Get_cheatsheet_cell(2,2).Should().Be(2);
            _Sut.Get_cheatsheet_cell(3,2).Should().Be(1);
                                                   
            _Sut.Get_cheatsheet_cell(0,3).Should().Be(0);
            _Sut.Get_cheatsheet_cell(1,3).Should().Be(0);
            _Sut.Get_cheatsheet_cell(2,3).Should().Be(0);
            _Sut.Get_cheatsheet_cell(3,3).Should().Be(0);
        }

        private void WriteToDisk(string content)
        {
            _Input_Filename = _TempDirectory + Path.DirectorySeparatorChar + "MinesweeperUnitTests";
            File.WriteAllText(_Input_Filename, content);
        }
    }
}
